using System.Web.Mvc;
using SimpleChat.DataAccess;

namespace SimpleChat.Utils
{
    public class RemoveOldChatroomsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repository = DependencyResolver.Current.GetService<ISimpleChatRepository>();
            repository.RemoveOldChatrooms();
        }
    }
}