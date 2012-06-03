using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleChat.Models;

namespace SimpleChat.DataAccess
{
    public interface ISimpleChatRepository
    {
        void RemoveOldChatrooms();
        string BuildChatUrl();
        bool CheckChatUrl(string toCheck);
        void CreateChatroom(Chatroom toCreate);
        Chatroom FindByUrl(string url);
        Chatroom FindByID(string id);

        void UpdateParticipant(string chatId, string sessionId, string name);
        string GetParticipantName(string chatId, string sessionId);
        List<Participant> GetParticipants(string ChatId);

        void AddMessage(Message toAdd);
        List<Message> FetchMessages(string ChatId);
        //List<Message> FetchMessagesAfter(int ChatId, int MessageId);
        List<Message> FetchMessagesAfter(string ChatUrl, string MessageId);
    }
}
