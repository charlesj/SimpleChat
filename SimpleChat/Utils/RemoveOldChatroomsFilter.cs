using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleChat.Models;
using SimpleChat.DataAccess;

namespace SimpleChat.Utils
{
    public class RemoveOldChatroomsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repository = System.Web.Mvc.DependencyResolver.Current.GetService<ISimpleChatRepository>();
            repository.RemoveOldChatrooms();
        }
    }
}