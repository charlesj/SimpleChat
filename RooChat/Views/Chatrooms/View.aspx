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
    <div id="messages-container">&nbsp;
        <div id="message-sent" class="app-message">Message Sent</div>
        <div id="message-error" class="app-message">There was a problem sending the message</div>
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
                    type: "POST",
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
           

            function fetchMessages(){
                var url = "/Chatrooms/FetchMessages/<%= Model.Chatroom.Id %>/";
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

            var periodicCheck = setInterval(fetchMessages, 1000);

            //do this once the page loads as well: 
            $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);
        });
    </script>
</asp:Content>
