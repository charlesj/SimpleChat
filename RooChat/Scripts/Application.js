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
        var url = "/Chatrooms/FetchMessages/" + FindCurrentChatUrl() + "/" + FindLastMessageId();
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