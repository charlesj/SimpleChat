using System;
using System.Collections.Generic;
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
        List<Participant> GetParticipants(int chatId);

        void AddMessage(Message toAdd);
        List<Message> FetchMessages(int chatId);
        List<Message> FetchMessagesAfter(string chatUrl, int messageId);
    }
}
