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

        public bool HasValidUrl()
        {
            return (Url != null && Url != "Default" && Url != string.Empty && !UrlExists(Url));
        }

        public void BuildUrl()
        {
            int url_length = 3;
            string characters = "0123456789abcdefghijklmnopqrstuvwxyz";
            Random r = new Random(DateTime.Now.Millisecond);
            var sb = new StringBuilder();
            for (int i = 0; i < url_length; i++)
            {
                sb.Append(characters[r.Next(characters.Length - 1)]);
            }
            while( Chatroom.UrlExists(sb.ToString()))
            {
                sb = new StringBuilder();
                for (int i = 0; i < url_length; i++)
                {
                    sb.Append(characters[r.Next(characters.Length - 1)]);
                }
            }
            Url = sb.ToString();
            //Url = Convert.ToBase64String(BitConverter.GetBytes(Id));
            //return Url;
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
