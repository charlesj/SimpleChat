<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RooChat: <%= ((RooChat.Models.Chatroom)ViewData["chatroom"]).Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <% var chatroom = (RooChat.Models.Chatroom)ViewData["chatroom"]; %>
    <div id="chatroom-url"><strong>url:</strong> <a href="/<%= chatroom.Url %>">http://chat.umkc.edu/<%= chatroom.Url %></a></div>
    <h2><%= chatroom.Name %></h2>
    <div id="participants">
        
    </div>
    <div id="conversation-container">
        <div id="conversation">

        </div>
    </div>
    <div id="send-message">
        <form id="message-form" action="/Chatrooms/AddMessage/<%= chatroom.Id %>" method="post">
            <fieldset>
            <div style="float: right;"><a href="/Chatrooms/Transcript/<%= chatroom.Id %>">Download Transcript</a></div>
            <legend>Send Message</legend>
            <label for="name">Name</label>
            <input type="text" id="name" name="name" value="YourName" /> 
                            <div id="message-sent" class="app-message">Message Sent</div>
                <div id="message-error" class="app-message">There was a problem sending the message</div>
            <br />
            <textarea id="message" rows="5" cols="100" name="message" style="background-color: #d6e5ff; border:1px dashed #556688;"></textarea>
            <br />
            <input type="submit" id="submit-button" value="Send" name="submit-button" />
            </fieldset>
        </form>
    </div>
    <script language="javascript" type="text/javascript">
        var last_id = <%= ViewData["last_id"] %>;
        $(document).ready(function () {


            $("#message-form").submit(function () {
                var f = $("#message-form");
                var action = f.attr("action");
                var sform = f.serialize();
                //                $.post(action, sform,
                //                    function (response) {
                //                        $("#message-sent").text(response).css('display', 'inline').show('slow');
                //                    }
                //                );
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
                var url = "/Chatrooms/FetchMessages/<%= chatroom.Id %>/" + last_id;
                //we can also define the chatroom id earlier
                //build the url here
                $.ajax({
                    method: 'get',
                    url: url,
                    dataType: 'text',
                    success: function (text) { $('#conversation').append(text); }
                });
            }
            var holdTheInterval = setInterval(checkForMessages, 2000);
        });
    </script>
</asp:Content>
