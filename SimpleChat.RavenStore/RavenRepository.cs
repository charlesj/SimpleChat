using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using SimpleChat.DataAccess;
using SimpleChat.Models;
using SimpleChat.Utilities;

namespace SimpleChat.RavenStore
{
    public class RavenRepository : ISimpleChatRepository
    {
        //works per session
        //private IDocumentSession session;
        private readonly IDocumentStore _store;

        public RavenRepository(IDocumentStore store)
        {
            _store = store;
        }

        public void RemoveOldChatrooms()
        {
            using (var session = _store.OpenSession())
            {
                var chatrooms = session.Query<Chatroom>().Where(c => c.LastActionOn < 3.Hours().Ago());
                foreach (var chatroom in chatrooms)
                {
                    var chat = chatroom;
                    //grab paricipants to delete
                    var parts = session.Query<Participant>().Where(p => p.ChatroomId == chat.Id);
                    foreach (var part in parts)
                    {
                        session.Delete(part);
                    }
                    //grab messages
                    var messages = session.Query<Message>().Where(m => m.ChatId == chat.Id);
                    foreach (var message in messages)
                    {
                        session.Delete(message);
                    }
                    session.Delete(chatroom);
                }
                session.SaveChanges();
            }
        }

        public string BuildChatUrl()
        {
            var test = StringIDGenerator.GetId(7);
            while (CheckChatUrl(test))
            {
                test = StringIDGenerator.GetId(7);
            }
            return test;

        }

        public bool CheckChatUrl(string toCheck)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Chatroom>().Any(c => c.Url == toCheck);
            }
        }

        public Chatroom CreateChatroom(string name, string url)
        {
            using (var session = _store.OpenSession())
            {
                var chat = new Chatroom
                               {
                                   Name = name,
                                   CreatedOn = DateTime.Now,
                                   LastActionOn = DateTime.Now,
                                   Url = string.IsNullOrEmpty(url) ? BuildChatUrl() : url
                               };
                session.Store(chat);
                session.SaveChanges();
                return chat;
            }
        }

        public Chatroom FindByUrl(string url)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Chatroom>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfNow(5.Seconds()))
                    .Single(c => c.Url == url);
            }
        }

        public Chatroom FindByID(int id)
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<Chatroom>(id);
            }
        }

        public void UpdateParticipant(int chatId, string sessionId, string name)
        {
            using (var session = _store.OpenSession())
            {
                //find if exists
                var participant = session.Query<Participant>().SingleOrDefault(p => p.ChatroomId == chatId && p.SessionId == sessionId);
                if (participant == null)
                {
                    participant = new Participant
                                      {ChatroomId = chatId, SessionId = sessionId, Name = name, LastSeen = DateTime.Now};
                    session.Store(participant);
                }
                else
                {
                    participant.LastSeen = DateTime.Now;
                    if (string.IsNullOrEmpty(participant.Name) || name !="Big Nose") //Only change it if it's not big nose
                    {
                        participant.Name = name;
                    }
                }

                session.SaveChanges();
            }
            UpdateChatLastAction(chatId);
        }

        public void UpdateChatLastAction(int chatId)
        {
            using (var session = _store.OpenSession())
            {
                var chat = session.Load<Chatroom>(chatId);
                chat.LastActionOn = DateTime.Now;
                session.SaveChanges();
            }
        }

        public string GetParticipantName(int chatId, string sessionId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Participant>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfNow(5.Seconds()))
                    .Single(p => p.SessionId == sessionId && p.ChatroomId == chatId).Name;
            }
        }

        public List<Participant> GetParticipants(int chatId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Participant>().Where(p => p.ChatroomId == chatId).OrderBy(x => x.Name).ToList();
            }
        }

        public void AddMessage(Message toAdd)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(toAdd);
                session.SaveChanges();
            }
        }

        public List<Message> FetchMessages(int chatId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Message>().Where(m => m.ChatId == chatId).OrderBy(x => x.Id).ToList();
            }
        }

        public List<Message> FetchMessagesAfter(string chatUrl, int messageId)
        {
            using (var session = _store.OpenSession())
            {
                var chat = FindByUrl(chatUrl);
                var rtn = session.Query<Message>().Where(m => m.ChatId == chat.Id && m.Id > messageId).ToList();
                return rtn;
            }
        }

        public void Dispose()
        {
            //session.Dispose();
        }
    }
}
