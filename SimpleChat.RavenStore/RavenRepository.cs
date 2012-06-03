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
    public class RavenRepository : ISimpleChatRepository
    {
        //works per session
        private IDocumentSession session;

        public RavenRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void RemoveOldChatrooms()
        {
            var chatrooms = session.Query<Chatroom>().Where(c => c.LastActionOn < 30.Days().Ago());
            foreach (var chatroom in chatrooms)
            {
                session.Delete<Chatroom>(chatroom);
            }
            session.SaveChanges();
        }

        public string BuildChatUrl()
        {
            var gen = new StringIDGenerator();
            return StringIDGenerator.GetId(7);
        }

        public bool CheckChatUrl(string toCheck)
        {
            return session.Query<Chatroom>().Any(c => c.Url == toCheck);
        }

        public void CreateChatroom(Chatroom toCreate)
        {
            session.Store(toCreate);
            session.SaveChanges();
        }

        public Chatroom FindByUrl(string url)
        {
            return session.Query<Chatroom>().Where(c => c.Url == url).Single();
        }

        public Chatroom FindByID(string id)
        {
            return session.Load<Chatroom>(id);
        }

        public void UpdateParticipant(string chatId, string sessionId, string name)
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
            }
            
            session.SaveChanges();
        }

        public string GetParticipantName(string chatId, string sessionId)
        {
            return session.Query<Participant>().Where(p => p.SessionId == sessionId && p.ChatroomId == chatId).Single().Name;
        }

        public List<Participant> GetParticipants(string ChatId)
        {
            return session.Query<Participant>().Where(p => p.ChatroomId == ChatId).ToList();
        }

        public void AddMessage(Message toAdd)
        {
            session.Store(toAdd);
            session.SaveChanges();
        }

        public List<Message> FetchMessages(string ChatId)
        {
            return session.Query<Message>().Where(m => m.ChatId == ChatId).ToList();
        }

        public List<Message> FetchMessagesAfter(string ChatUrl, string MessageId)
        {
            var chat = this.FindByUrl(ChatUrl);
            return session.Query<Message>().Where(m => m.ChatId == chat.Id).ToList();
        }
    }
}
