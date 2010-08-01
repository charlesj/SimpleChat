using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RooChat.Models;

namespace RooChat.Controllers
{
    public class ChatroomsController : Controller
    {
        //
        // GET: /Chatrooms/

        public ActionResult Index()
        {
            return View();
        }

        public RedirectResult Create(string name, string url)
        {
            if (url == string.Empty || url == null)
                url = "PleaseSetThisAfterFirstSave";
            if (name == string.Empty || name == null)
                name = "RooChat";

            if (Chatroom.UrlExists(url))
            {
                TempData["error"] = "This url is already in use";
                return Redirect("/");
            }
            
            var db = new RooChatDataContext();
            var chat = new Chatroom
            {
                Name = name,
                Url = url,
                CreatedOn = DateTime.Now
            };

            db.Chatrooms.InsertOnSubmit(chat);
            db.SubmitChanges();
            if (chat.Url == "PleaseSetThisAfterFirstSave")
                chat.BuildUrl();
            db.SubmitChanges();
            return Redirect("/" + chat.Url);
        }

        public ActionResult View(string path)
        {
            if (path == "Default")
                return RedirectToAction("Index", "Chatrooms", new { id=1 });
            var chat = Chatroom.FindByUrl(path);
            ViewData["chatroom"] = chat;
            ViewData["last_id"] = chat.Messages.Last().Id;
            return View();
        }
        
        [ValidateInput(false)]
        public string AddMessage(int id, string name, string message)
        {
            var db = new RooChatDataContext();
            var mess = new Message
            {
                Chat_id = id,
                Name = name,
                Content = message,
                SentOn = DateTime.Now,
                Ip = "NotLogged"
            };
            db.Messages.InsertOnSubmit(mess);
            db.SubmitChanges();
            return "Message Received";
        }

        public ActionResult FetchMessages(int chat_id, int last_m_id)
        {
            var messages = Message.FetchAfter(last_m_id, chat_id);
            var lid = messages[messages.Count - 1].Id;
            ViewData["messages"] = messages;
            ViewData["lid"] = lid;
            return View();
        }

        public ActionResult Transcript(int id)
        {
            var chat = Chatroom.Find(id);
            Response.AddHeader("content-disposition", "attachment; filename=Transcript-" + chat.Name + ".html");
            ViewData["chat"] = chat;
            ViewData["messages"] = chat.Messages.ToList();
            return View();
        }
    }
}
