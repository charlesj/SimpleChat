using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RooChat.Models
{
    public partial class Participant
    {

        public static void UpdateParticipant(int chat_id, string session_id, string name)
        {
            var db = new RooChatDataContext();
            var p = (from participants in db.Participants
                     where participants.Chatroom_id == chat_id &&
                        participants.Session_id == session_id
                     select participants).SingleOrDefault();
            if( p == null )
            {
                // new participant
                p = new Participant();
                p.Session_id = session_id;
                p.Chatroom_id = chat_id;
                p.Name = name;
                p.Last_seen = DateTime.Now;
                db.Participants.InsertOnSubmit(p);
                db.SubmitChanges();
            }
            else
            {
                //update participant
                if (name != "Unnamed")
                {
                    p.Name = name;
                }

                p.Last_seen = DateTime.Now;
                db.SubmitChanges();
            }
              
        }

        public static string GetParticipantName(string session_id)
        {
            var db = new RooChatDataContext();
            var ip = (from participants in db.Participants
                      where participants.Session_id == session_id
                      select participants).ToList();
            if (ip.Count > 0)
                return ip.Last().Name;
            else
                return "Unnamed";
        }
    }
}
