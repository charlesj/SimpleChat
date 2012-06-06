using System;

namespace SimpleChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}
