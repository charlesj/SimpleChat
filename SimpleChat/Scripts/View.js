$(document).ready(function () {
    //document.write(window.location.pathname);
    //finish setup for javascript handling
    //hide submit button and messages
    $("#messages-container").append('<span id="processing"><img src="/Content/processing.gif" alt="processing" /> Sending...</span>');
    $("#messages-container").append('<span id="message-sent" class="app-message">Message Sent</span>');
    $("#messages-container").append('<span id="message-error" class="app-message">There was a problem sending the message</span>');
    $("#processing").hide();
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
    $(".message:odd").addClass('odd');


    //this submits the form over ajax
    $("#message-form").submit(function () {
        var f = $("#message-form");
        var action = f.attr("action");
        var sform = f.serialize();
        SendingMessage();
        $.ajax({
            url: action,
            success: function (response) { $("#message-sent").text(response).css('display', 'inline').show().delay(5000).fadeOut('slow'); },
            data: sform,
            type: "POST",
            error: function () { $("#message-error").css('display', 'inline').show().delay(5000).fadeOut('slow'); },
            complete: function () { MessageSent() }
        });
        return false;
    });

    function SendingMessage() {
        $("#message").attr("disabled", "disabled");
        $("#processing").show();
    }
    function MessageSent() {
        $("#message").removeAttr("disabled");
        $("#processing").hide();
    }

    function FindCurrentChatUrl() {
        return window.location.pathname.replace("/", "");
    }

    function FindLastMessageId() {
        //var mid = $(".message:last").attr('id').replace("m-", "");
        if ($(".message:last").length == 0) {
            return -1;
        }
        else {
            return $(".message:last").attr('id').replace("m-", "");
        }

    }

    //Periodical check for messages
    var executing_fetch = false; //this variable insure that we only have one async request at a time
    function fetchMessages() {
        if (!executing_fetch) {
            executing_fetch = true;
            var fetch_action = "/Chatrooms/FetchMessages/" + FindCurrentChatUrl() + "/" + FindLastMessageId();
            $.ajax({
                url: fetch_action,
                success: function (response) {
                    $("#conversation").append(response);
                    if (response.length > 10) {
                        // shouldn't force scrolling unless there is a new message
                        // this behavior is far from ideal, so we may want to abstract this out into another method that 
                        // can better control scrolling.
                        $("#conversation-container").scrollTop($("#conversation-container")[0].scrollHeight);
                    }
                    $(".message:odd").addClass('odd');
                },
                type: "GET",
                error: function () { $("#message-error").css('display', 'inline').show().delay(5000).fadeOut('slow'); },
                complete: function () { executing_fetch = false; } //let them know they can execute another request
            });
        }
    }
    //this actually sets the check for messages.  We're going to check it once a second.
    var periodicCheck = setInterval(fetchMessages, 3000); //actually we're not that worried about performance.

    //Periodical for handling participants
    var executing_particpants = false;
    function fetchParticipants() {
        if (!executing_particpants) {
            executing_particpants = true;
            var url = "/Chatrooms/Participants/" + FindCurrentChatUrl() + "/" + $("#name").val();
            $.ajax({
                url: url,
                success: function (response) { $("#participants").html(response); },
                type: "GET",
                complete: function () { executing_particpants = false }
            });
        }
    }
    var participantCheck = setInterval(fetchParticipants, 3000);
});