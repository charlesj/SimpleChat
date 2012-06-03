using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Models
{
    public class Participant
    {
        public string Id { get; set; }
        public string ChatroomId { get; set; }
        public string SessionId { get; set; }
        public string Name { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
