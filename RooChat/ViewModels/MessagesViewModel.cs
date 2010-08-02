using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RooChat.Models;
using MarkdownSharp;

namespace RooChat.Views
{
    public class MessagesViewModel
    {
        public List<Message> Messages { get; set; }
        public int LastId { get; set; }
        public Chatroom Chatroom { get; set; }
        public Markdown Markdown { get; private set; }

        public MessagesViewModel()
        {
            Markdown = new Markdown();
            Markdown.AutoHyperlink = true;
        }
    }
}