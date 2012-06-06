using System;
using System.Web.Mvc;
using SimpleChat.Models;
using SimpleChat.Utils;
using SimpleChat.DataAccess;
using SimpleChat.ViewModels;

namespace SimpleChat.Controllers
{
    public class ChatroomsController : Controller
    {
        private readonly ISimpleChatRepository _repository;

        public ChatroomsController(ISimpleChatRepository repository)
        {
            //repository = new RavenRepository(MvcApplication.Store);
            _repository = repository;
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
            if (string.IsNullOrEmpty(name))
                name = "Simple Chat";

            if (!string.IsNullOrEmpty(url) && _repository.CheckChatUrl(url))
            {
                TempData["error"] = "This url is already in use";
                return Redirect("/");
            }

            var chat = _repository.CreateChatroom(name, url);
            return Redirect("/" + chat.Url);
        }

        [RemoveOldChatroomsFilter]
        [ActionName("View")]
        public ActionResult ViewMethod(string path)
        {
            try
            {
                if (path == "Default")
                    return RedirectToAction("Index", "Chatrooms", new { id = 1 });

                var chat = _repository.FindByUrl(path);

                _repository.UpdateParticipant(chat.Id, Session.SessionID, "Big Nose");
                var name = _repository.GetParticipantName(chat.Id, Session.SessionID);
                var vm = new ChatroomViewModel
                             {
                                 ActiveParticipants = _repository.GetParticipants(chat.Id),
                                 CreatedOn = chat.CreatedOn,
                                 Id = chat.Id,
                                 LastActionOn = chat.LastActionOn,
                                 Messages = _repository.FetchMessages(chat.Id),
                                 Name = chat.Name,
                                 Url = chat.Url,
                                 ParticipantName = name
                             };
                return View(vm);
            }
            catch
            {
                return HttpNotFound("The chatroom you're looking for could not be found - or has not yet finished being created.  You might try again in a few seconds");
            }
        }

        public ActionResult AddMessage(int id, string name, string message)
        {
            var chat = _repository.FindByID(id);
            string returnMessage;
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
                _repository.UpdateParticipant(chat.Id, Session.SessionID, name);
                _repository.AddMessage(mess);
                returnMessage = "Message Received";
            }
            else
            {
                returnMessage = "Ignoring Empty Message";
            }
            if (Request.IsAjaxRequest())
            {
                return new ContentResult { Content = returnMessage };
            }
            TempData["message"] = returnMessage;
            return Redirect("/" + chat.Url);
        }

        public ViewResult FetchMessages(string chat_url, int last_m_id)
        {
            //var messages = Message.FetchFor(chat_id);
            var messages = _repository.FetchMessagesAfter(chat_url, last_m_id);
            //System.Threading.Thread.Sleep(3000); //Simulate a high latency connection
            return View(messages);
        }

        public ActionResult Transcript(int id)
        {
            var chat = _repository.FindByID(id);
            Response.AddHeader("content-disposition", "attachment; filename=Transcript-" + chat.Name + ".html");
            ViewData["chat"] = chat;
            ViewData["messages"] = _repository.FetchMessages(chat.Id);
            return View();
        }

        public ActionResult Participants(string url, string name)
        {
            var chat = _repository.FindByUrl(url);
            _repository.UpdateParticipant(chat.Id, Session.SessionID, name);
            return View(_repository.GetParticipants(chat.Id));
        }
    }
}
