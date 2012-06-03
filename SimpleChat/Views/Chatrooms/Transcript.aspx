<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <% var chat = (SimpleChat.Models.Chatroom)ViewData["chat"];
       var messages = (List<SimpleChat.Models.Message>)ViewData["messages"];
       var markup = new MarkdownDeep.Markdown();
       //markup.AutoHyperlink = true;
    %>
    <title>Transcript for <%= chat.Name %></title>
    <style>
        .message{margin: 0px; clear: both; }
        .message h3{ margin:0px; padding:0px; font-size: 10px; float: left; width: 50px; text-align:right; }
        .message-content{ margin-left: 55px;border-left: 1px solid #000; padding-left: 3px;  border-bottom: 1px dotted #000;}
        .message p{margin:0; padding-bottom: 5px;}
    </style>
</head>
<body>
    <h1>Transcript for <%= chat.Name %></h1>
    <% foreach (var message in messages)
       {%>
          <div class="message" id="m-<%= message.Id %>">
            <h3><%= message.Name %></h3>
            <div class="message-content"><%= markup.Transform(message.Content) %></div>
               <a name="message-<%= message.Id %>"></a>
           </div>
    <% } %>
</body>
</html>
