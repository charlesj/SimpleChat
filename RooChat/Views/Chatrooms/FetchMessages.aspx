<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<% var messages = (List<RooChat.Models.Message>)ViewData["messages"];
   var markup = new MarkdownSharp.Markdown();
   markup.AutoHyperlink = true; %>
<% foreach (var message in messages)
   { %>
   <div class="message" id="m-<%= message.Id %>">
    <h3><%= message.Name %></h3>
    <div class="message-content">
    <div class="message-time"><%= message.SentOn.ToShortTimeString() %></div>
    <%= markup.Transform(message.Content) %></div>
   </div>

<% } %>
<script language="javascript">
    last_id=<%= ViewData["lid"] %>;
    $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);
</script>