using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Models
{
    public partial class Chatroom
    {
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActionOn { get; set; }
    }
}
