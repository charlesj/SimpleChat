using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using SimpleChat.DataAccess;
using SimpleChat.Models;
using SimpleChat.Utilities;

namespace SimpleChat.RavenStore
{
    public class RavenRepository : ISimpleChatRepository, IDisposable
    {
        //works per session
        //private IDocumentSession session;
        private IDocumentStore store;

        public RavenRepository(IDocumentStore store)
        {
            this.store = store;
        }

        public void RemoveOldChatrooms()
        {
            using (var session = store.OpenSession())
            {
                var chatrooms = session.Query<Chatroom>().Where(c => c.LastActionOn < 3.Days().Ago());
                foreach (var chatroom in chatrooms)
                {
                    session.Delete<Chatroom>(chatroom);
                }
                session.SaveChanges();
            }
        }

        public string BuildChatUrl()
        {
            using (var session = store.OpenSession())
            {
                var gen = new StringIDGenerator();
                var test = StringIDGenerator.GetId(7);
                while (CheckChatUrl(test))
                {
                    test = StringIDGenerator.GetId(7);
                }
                return test;
            }
        }

        public bool CheckChatUrl(string toCheck)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Chatroom>().Any(c => c.Url == toCheck);
            }
        }

        public Chatroom CreateChatroom(string name, string url)
        {
            using (var session = store.OpenSession())
            {
                var chat = new Chatroom { 
                    Name = name, 
                    CreatedOn = DateTime.Now,  
                    LastActionOn = DateTime.Now 
                };
                if (string.IsNullOrEmpty(url))
                {
                    chat.Url = BuildChatUrl();
                }
                else
                {
                    chat.Url = url;
                }
                session.Store(chat);
                session.SaveChanges();
                return chat;
            }
        }

        public Chatroom FindByUrl(string url)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Chatroom>().Where(c => c.Url == url).Single();
            }
        }

        public Chatroom FindByID(int id)
        {
            using (var session = store.OpenSession())
            {
                return session.Load<Chatroom>(id);
            }
        }

        public void UpdateParticipant(int chatId, string sessionId, string name)
        {
            using (var session = store.OpenSession())
            {
                //find if exists
                var participant = session.Query<Participant>().Where(p => p.ChatroomId == chatId && p.SessionId == sessionId).SingleOrDefault();
                if (participant == null)
                {
                    participant = new Participant { ChatroomId = chatId, SessionId = sessionId, Name = name };
                    participant.LastSeen = DateTime.Now;
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
        }

        public string GetParticipantName(int chatId, string sessionId)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Participant>().Where(p => p.SessionId == sessionId && p.ChatroomId == chatId).Single().Name;
            }
        }

        public List<Participant> GetParticipants(int ChatId)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Participant>().Where(p => p.ChatroomId == ChatId).OrderBy(x => x.Name).ToList();
            }
        }

        public void AddMessage(Message toAdd)
        {
            using (var session = store.OpenSession())
            {
                session.Store(toAdd);
                session.SaveChanges();
            }
        }

        public List<Message> FetchMessages(int ChatId)
        {
            using (var session = store.OpenSession())
            {
                return session.Query<Message>().Where(m => m.ChatId == ChatId).ToList();
            }
        }

        public List<Message> FetchMessagesAfter(string ChatUrl, int MessageId)
        {
            using (var session = store.OpenSession())
            {
                var chat = this.FindByUrl(ChatUrl);
                var rtn = session.Query<Message>().Where(m => m.ChatId == chat.Id && m.Id > MessageId).ToList();
                return rtn;
            }
        }

        public void Dispose()
        {
            //session.Dispose();
        }
    }
}
