<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Welcome to SimpleChat!
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <p>Welcome to SimpleChat, the easiest way to setup and use a chatroom.  Here's the process:</p>
    <ol>
        <li>Enter a name for your chatroom</li>
        <li>If you wish, enter a url.  This is what will appear after chat.umkc.edu.  Example:
            <blockquote><%= Request.Url.Host %>/MostExcellentChatroom</blockquote>
        </li>
        <li>Send full url to those who want to join in the chat.</li>
        <li>Chatrooms are automatically deleted after 3 days of non-use.</li>
    </ol>
    <h2>Create Chatroom</h2>
    <div id="create-form">
    <% Html.BeginForm("Create", "Chatrooms"); %>
    <p><strong>Chatroom Name</strong></p>
    <%= Html.TextBox("name") %>
    <p><strong>Url</strong> (Optional)</p>
    <%= Request.Url.Host %>/<%= Html.TextBox("url") %>
    <p></p>
    <input type="submit" value="Create" />
    <% Html.EndForm(); %>
    </div>
</asp:Content>
