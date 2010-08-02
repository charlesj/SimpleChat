using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RooChat.Models
{
    public partial class Message
    {
        public static List<Message> FetchAfter(int message_id, string chat_url)
        {
            var db = new RooChatDataContext();
            return (from messages in db.Messages
                    where messages.Id > message_id && messages.Chatroom.Url == chat_url
                    select messages).ToList();
        }
        public static Message Find(int message_id)
        {
            var db = new RooChatDataContext();
            return (from messages in db.Messages
                    where messages.Id == message_id
                    select messages).SingleOrDefault();
        }
    }
}
