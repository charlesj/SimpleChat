using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleChat.Models;

namespace SimpleChat.Utils
{
    public class RemoveOldChatroomsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Chatroom.RemoveOldRooms();
        }
    }
}