<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SimpleChat.Models.Message>>" %>
<% foreach (var message in Model)
   { %>
    <% Html.RenderPartial("Message", message); %>
<% } %>