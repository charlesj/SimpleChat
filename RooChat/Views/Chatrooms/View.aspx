<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RooChat.Models.Chatroom>" %>
<%@ Import Namespace="System.Data.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RooChat: <%= Model.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="chatroom-url"><strong>url:</strong> <a href="/<%= Model.Url %>">http://chat.umkc.edu/<%= Model.Url %></a></div>
    <h2><%= Model.Name %></h2>
    <p id="NoJavascriptMessages"><strong>Your browser seems to have javascript disabled.  You will need to refresh the page to view any new messages.</strong></p>
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
            <input type="text" id="name" name="name" value="YourName" /> 
            <br />
            <textarea id="message" rows="5" cols="100" name="message" style="background-color: #d6e5ff; border:1px dashed #556688;"></textarea>
            <br />
            <input type="submit" id="submit-button" value="Send" name="submit-button" />
            
            <div id="messages-container">&nbsp;
            </div>
        </form>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //document.write(window.location.pathname);
            //finish setup for javascript handling
            //hide submit button and messages
            $("#messages-container").append('<span id="message-sent" class="app-message">Message Sent</div>');
            $("#messages-container").append('<span id="message-error" class="app-message">There was a problem sending the message</div>');
            $("#message-sent").hide();
            $("#message-error").hide();
            $("#submit-button").hide();
            $("#NoJavascriptMessages").hide();

            //setup using the return key to send message
            $("#message").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#message-form").submit();
                    $("#message").val('');
                }
            });

            //conversation-container needs a height and scroll if we're using javascript
            //if not, we want it to display without these things.
            $("#conversation-container").addClass('conversation-container-javascript');
            //do this once the page loads to make sure we see the last message
            $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);

            //this submits the form over ajax
            $("#message-form").submit(function () {
                var f = $("#message-form");
                var action = f.attr("action");
                var sform = f.serialize();
                $.ajax({
                    url: action,
                    success: function (response) { $("#message-sent").text(response).css('display', 'inline').show().delay(5000).fadeOut('slow'); },
                    data: sform,
                    type: "POST",
                    error: function(){ $("#message-error").css('display', 'inline').show().delay(5000).fadeOut('slow'); },
                });
                return false;
            });

            function FindCurrentChatUrl()
            {   
                return window.location.pathname.replace("/","");
            }

            function FindLastMessageId()
            {
                return $(".message:last").attr('id').replace("m-","");
            }

            //Periodical check for messages
            function fetchMessages(){
                var url = "/Chatrooms/FetchMessages/<%= Model.Id %>/";
                //Fetch Message Id List
                $.getJSON(url, null, function(data){
                    //parse json
                    $.each(data, function(index, singleLine) {
                        //Insure that they are all displayed.
                        var check_id = "#m-" + singleLine.message_id;
                        if( $(check_id).length < 1 ){
                            var message_url = "/chatrooms/FetchMessage/" + singleLine.message_id;
                            //If not, display them
                            $.ajax({
                                method: 'get',
                                url: message_url,
                                dataType: 'text',
                                success: function (text) { $('#conversation').append(text); }
                            });
                        }
                    });
                });

                //We still need to scroll to the bottom of the thing
                $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);
            }
            //this actually sets the check for messages.  We're going to check it once a second.
            var periodicCheck = setInterval(fetchMessages, 1000);
        });
    </script>
</asp:Content>
