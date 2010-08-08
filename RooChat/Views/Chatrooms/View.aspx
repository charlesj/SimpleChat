<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RooChat.Models.Chatroom>" %>
<%@ Import Namespace="System.Data.Linq" %>
<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
	RooChat: <%= Model.Name %>
</asp:Content>
<asp:Content ID="ScriptIncludes" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="/scripts/View.js" type="text/javascript" ></script>
</asp:Content>
<asp:Content ID="MainPage" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chatroom-url"><strong>url:</strong> <a href="/<%= Model.Url %>">http://chat.umkc.edu/<%= Model.Url %></a></div>
    <h2><%= Model.Name %></h2>
    
    <p id="NoJavascriptMessages"><strong>Your browser seems to have javascript disabled.  You will need to refresh the page to view any new messages.</strong></p>
    
    <div id="participants">
        <% Html.RenderPartial("Participants", Model.ActiveParticipants); %>
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
            <input type="text" id="name" name="name" value="Unnamed" /> 
            <br />
            <textarea id="message" rows="5" cols="100" name="message" style="background-color: #d6e5ff; border:1px dashed #556688;"></textarea>
            <br />
            <input type="submit" id="submit-button" value="Send" name="submit-button" />
        </form>
        <div id="messages-container">&nbsp;</div>
    </div>
</asp:Content>