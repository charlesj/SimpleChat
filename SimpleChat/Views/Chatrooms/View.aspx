<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SimpleChat.Models.Chatroom>" %>
<%@ Import Namespace="System.Data.Linq" %>
<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
	SimpleChat: <%= Model.Name %>
</asp:Content>
<asp:Content ID="ScriptIncludes" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="/scripts/View.js" type="text/javascript" ></script>
</asp:Content>
<asp:Content ID="MainPage" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chatroom-url"><strong>url:</strong> <a href="/<%= Request.Url.ToString() %>"><%= Request.Url.ToString() %></a></div>
    <h2><%= Model.Name %></h2>
    
    <p id="NoJavascriptMessages"><strong>Your browser seems to have javascript disabled.  You will need to refresh the page to view any new messages.</strong></p>
    
    <div id="participants-container">
        <h3>Participants</h3>
        <div id="participants">
            <% Html.RenderPartial("Participants", Model.ActiveParticipants); %>
        </div>
    </div>
    <div id="conversation-container">
        <div id="conversation">
         <% Html.RenderPartial("MessagesDisplay", Model.Messages.ToList()); %>
        </div>
    </div>
    <div style="float: right;"><a href="/Chatrooms/Transcript/<%= Model.Id %>">Download Transcript</a></div>
    <div id="send-message">
        <form id="message-form" action="/Chatrooms/AddMessage/<%= Model.Id %>" method="post">
            <h2>Send Message</h2>
            <label for="name">Name</label>
            <input type="text" id="name" name="name" value="<%= (string)ViewData["name"] %>" /> 
            <br />
            <textarea id="message" rows="5" cols="100" name="message"></textarea>
            <p id="markdown">Use <a href="http://daringfireball.net/projects/markdown/basics">Markdown</a> for formating</p>
            <br />
            <input type="submit" id="submit-button" value="Send" name="submit-button" />
        </form>
        <div id="messages-container">&nbsp;</div>
    </div>
</asp:Content>