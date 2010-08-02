<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RooChat.Views.MessagesViewModel>" %>
<% foreach (var message in Model.Messages)
   { %>
   <div class="message" id="m-<%= message.Id %>">
    <h3><%= message.Name %></h3>
    <div class="message-content">
    <div class="message-time"><%= message.SentOn.ToShortTimeString() %></div>
    <%= Model.Markdown.Transform(message.Content) %></div>
   </div>
<% } %>