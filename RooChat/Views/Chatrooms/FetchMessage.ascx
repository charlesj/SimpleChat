<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RooChat.Models.Message>" %>
<% var markdown = new MarkdownSharp.Markdown();
   markdown.AutoHyperlink = true; %>
   <div class="message" id="m-<%= Model.Id %>">
    <h3><%= Model.Name %></h3>
    <div class="message-content">
    <div class="message-time"><%= Model.SentOn.ToShortTimeString() %></div>
    <%= markdown.Transform(Model.Content) %></div>
   </div>