using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RooChat.Models
{
    public partial class Message
    {
        public static List<Message> FetchAfter(int message_id, int conversation_id)
        {
            var db = new RooChatDataContext();
            return (from messages in db.Messages
                    where messages.Id > message_id && messages.Chat_id == conversation_id
                    select messages).ToList();
        }
    }
}
