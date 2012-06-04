using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleChat.Models;

namespace SimpleChat.DataAccess
{
    public interface ISimpleChatRepository: IDisposable
    {
        void RemoveOldChatrooms();
        string BuildChatUrl();
        bool CheckChatUrl(string toCheck);
        Chatroom CreateChatroom(string name, string url);
        Chatroom FindByUrl(string url);
        Chatroom FindByID(int chatId);

        void UpdateParticipant(int chatId, string sessionId, string name);
        string GetParticipantName(int chatId, string sessionId);
        List<Participant> GetParticipants(int ChatId);

        void AddMessage(Message toAdd);
        List<Message> FetchMessages(int ChatId);
        List<Message> FetchMessagesAfter(string ChatUrl, int MessageId);
    }
}
