using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RooChat.Models
{
    public partial class Chatroom
    {
        public static Chatroom FindByUrl(string url)
        {
            var db = new RooChatDataContext();
            return (from rooms in db.Chatrooms
                    where rooms.Url == url
                    select rooms).Single();
        }

        public static Chatroom Find(int id)
        {
            var db = new RooChatDataContext();
            return (from rooms in db.Chatrooms
                    where rooms.Id == id
                    select rooms).Single();
        }

        public static bool UrlExists(string url)
        {
            var db = new RooChatDataContext();
            return ((from rooms in db.Chatrooms
                     where rooms.Url == url
                     select rooms).Count() > 0);
        }

        public string BuildUrl()
        {
            Url = Convert.ToBase64String(BitConverter.GetBytes(Id));
            return Url;
        }


        public static void RemoveOldRooms()
        {
            var db = new RooChatDataContext();
            var rooms = from chatrooms in db.Chatrooms
                        where chatrooms.CreatedOn < DateTime.Now.AddDays(-3) && 
                            chatrooms.Messages.All(message => (message.SentOn < DateTime.Now.AddDays(-3)))
                        select chatrooms;
            
        }
    }
}
