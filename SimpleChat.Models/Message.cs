using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}
