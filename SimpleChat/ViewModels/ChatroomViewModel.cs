using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleChat.Models;

namespace SimpleChat.ViewModels
{
    public class ChatroomViewModel : Chatroom
    {
        public List<Message> Messages { get; set; }
        public List<Participant> ActiveParticipants { get; set; }
        public string ParticipantName { get; set; }
    }
}