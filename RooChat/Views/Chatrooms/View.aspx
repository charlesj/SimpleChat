<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RooChat.Views.MessagesViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RooChat: <%= Model.Chatroom.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="chatroom-url"><strong>url:</strong> <a href="/<%= Model.Chatroom.Url %>">http://chat.umkc.edu/<%= Model.Chatroom.Url %></a></div>
    <h2><%= Model.Chatroom.Name %></h2>
    <div id="participants">
        
    </div>
    <div id="conversation-container">
        <div id="conversation">
         <% Html.RenderPartial("MessagesDisplay", Model); %>
        </div>
    </div>
    <div id="messages-container">&nbsp;
        <div id="message-sent" class="app-message">Message Sent</div>
        <div id="message-error" class="app-message">There was a problem sending the message</div>
    </div>
    <div id="send-message">
        <form id="message-form" action="/Chatrooms/AddMessage/<%= Model.Chatroom.Id %>" method="post">
            <fieldset>
            <div style="float: right;"><a href="/Chatrooms/Transcript/<%= Model.Chatroom.Id %>">Download Transcript</a></div>
            <legend>Send Message</legend>
            <label for="name">Name</label>
            <input type="text" id="name" name="name" value="YourName" /> 
            <br />
            <textarea id="message" rows="5" cols="100" name="message" style="background-color: #d6e5ff; border:1px dashed #556688;"></textarea>
            <br />
            <input type="submit" id="submit-button" value="Send" name="submit-button" />
            </fieldset>
        </form>
    </div>
    <script language="javascript" type="text/javascript">
        var last_id = <%= Model.LastId %>;
        $(document).ready(function () {


            $("#message-form").submit(function () {
                var f = $("#message-form");
                var action = f.attr("action");
                var sform = f.serialize();
                $.ajax({
                    url: action,
                    success: function (response) { $("#message-sent").text(response).css('display', 'inline').show().delay(5000).fadeOut('slow'); },
                    data: sform,
                    error: function(){ $("#message-error").css('display', 'inline').show().delay(5000).fadeOut('slow'); },
                });
                return false;
            });

            $("#message-sent").hide();
            $("#message-error").hide();
            $("#submit-button").hide();

            $("#message").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#message-form").submit();
                    $("#message").val('');
                }
            });

            jQuery().ajaxStart(function () {
                $("#message-form").fadeOut("slow");
            });

            jQuery().ajaxStop(function () {
                $("#message-form").fadeIn("fast");
            });

            jQuery().ajaxError(function (e, xhr, settings, exception) {
                //$("message-error").text("asdf");//settings.url);
                $("#message-error").show();
            });


            //periodic updater
            function checkForMessages() {
                //we need to change the last message id on the fly
                //we can also define the chatroom id earlier
                //build the url here
                var url = "/Chatrooms/FetchMessages/<%= Model.Chatroom.Id %>/" + last_id;
                $.ajax({
                    method: 'get',
                    url: url,
                    dataType: 'text',
                    success: function (text) { $('#conversation').append(text); }
                });
                $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);
            }
            var holdTheInterval = setInterval(checkForMessages, 1000);
        });
    </script>
</asp:Content>
