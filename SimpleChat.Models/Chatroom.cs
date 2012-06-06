using System;

namespace SimpleChat.Models
{
    public class Chatroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActionOn { get; set; }
    }
}
