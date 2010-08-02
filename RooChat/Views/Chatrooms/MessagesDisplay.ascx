<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RooChat.Views.MessagesViewModel>" %>
<% foreach (var message in Model.Messages)
   { %>
    <% Html.RenderPartial("Fetchmessage", message); %>
<% } %>