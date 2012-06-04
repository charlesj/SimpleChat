using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client.Document;

namespace SimpleChat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore Store { get; set; }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "FetchMessages",
                "chatrooms/FetchMessages/{chat_url}/{last_m_id}",
                new { controller = "Chatrooms", action = "FetchMessages", chat_url = String.Empty, last_m_id = -1 }
            );

            routes.MapRoute(
                "Participants",
                "chatrooms/participants/{url}/{name}",
                new { controller = "Chatrooms", action = "Participants", url = "none", name = "Unnamed" }
            );

            routes.MapRoute(
                "Default", // Route name
                "chatrooms/{action}/{id}", // URL with parameters
                new { controller = "Chatrooms", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );



            routes.MapRoute(
                "Catchall",
                "{*path}",
                new { controller = "Chatrooms", action = "View", path = "Default" }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            Store = new DocumentStore
            {
                ConnectionStringName = "RavenDB",
            };
            Store.Conventions.IdentityPartsSeparator = "-";
            Store.Initialize();
        }

        protected void Session_Start()
        {
            //this insures that session id is available.
            var sid = Session.SessionID;
        }
    }
}