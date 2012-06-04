using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleChat.Models;
using SimpleChat.Utils;
using SimpleChat.DataAccess;
using SimpleChat.RavenStore;
using SimpleChat.ViewModels;
using System.Threading;

namespace SimpleChat.Controllers
{
    public class ChatroomsController : Controller
    {
        private ISimpleChatRepository repository;

        public ChatroomsController(ISimpleChatRepository repository)
        {
            //repository = new RavenRepository(MvcApplication.Store);
            this.repository = repository;
        }

        // GET: /Chatrooms/
        [RemoveOldChatroomsFilter]
        public ActionResult Index()
        {
            return View();
        }

        [RemoveOldChatroomsFilter]
        public RedirectResult Create(string name, string url)
        {
            if (name == string.Empty || name == null)
                name = "Simple Chat";

            if (!string.IsNullOrEmpty(url) && repository.CheckChatUrl(url))
            {
                TempData["error"] = "This url is already in use";
                return Redirect("/");
            }

            var chat = repository.CreateChatroom(name, url);
            Thread.Sleep(2000); // maybe this will help raven catch up;
            return Redirect("/" + chat.Url);
        }

        [RemoveOldChatroomsFilter]
        [ActionName("View")]
        public ActionResult View_Method(string path)
        {
            try
            {
                if (path == "Default")
                    return RedirectToAction("Index", "Chatrooms", new { id = 1 });

                var chat = repository.FindByUrl(path);

                repository.UpdateParticipant(chat.Id, Session.SessionID, "Big Nose");

                var vm = new ChatroomViewModel { ActiveParticipants = repository.GetParticipants(chat.Id), CreatedOn = chat.CreatedOn, Id = chat.Id, LastActionOn = chat.LastActionOn, Messages = repository.FetchMessages(chat.Id), Name = chat.Name, Url = chat.Url };
                vm.ParticipantName = "Big Nose";
                return View(vm);
            }
            catch
            {
                return HttpNotFound("The chatroom you're looking for could not be found - or has not yet finished being created.  You might try again in a few seconds");
            }
        }

        public ActionResult AddMessage(int id, string name, string message)
        {
            var chat = repository.FindByID(id);
            string return_message = string.Empty;
            if (message.Trim() != string.Empty)
            {
                var mess = new Message
                {
                    ChatId = id,
                    Name = name,
                    Content = message,
                    SentOn = DateTime.Now,
                    Ip = "NotLogged"
                };
                repository.UpdateParticipant(chat.Id, Session.SessionID, name);
                repository.AddMessage(mess);
                return_message = "Message Received";
            }
            else
            {
                return_message = "Ignoring Empty Message";
            }
            if (Request.IsAjaxRequest())
            {
                return new ContentResult { Content = return_message };
            }
            else
            {
                TempData["message"] = return_message;
                return Redirect("/" + chat.Url);
            }
        }

        public ViewResult FetchMessages(string chat_url, int last_m_id)
        {
            //var messages = Message.FetchFor(chat_id);
            var messages = repository.FetchMessagesAfter(chat_url, last_m_id);
            //System.Threading.Thread.Sleep(3000); //Simulate a high latency connection
            return View(messages);
        }

        public ActionResult Transcript(int id)
        {
            var chat = repository.FindByID(id);
            Response.AddHeader("content-disposition", "attachment; filename=Transcript-" + chat.Name + ".html");
            ViewData["chat"] = chat;
            ViewData["messages"] = repository.FetchMessages(chat.Id);
            return View();
        }

        public ActionResult Participants(string url, string name)
        {
            var chat = repository.FindByUrl(url);
            repository.UpdateParticipant(chat.Id, Session.SessionID, name);
            return View(repository.GetParticipants(chat.Id));
        }
    }
}
