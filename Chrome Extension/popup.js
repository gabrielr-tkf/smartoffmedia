// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.



var guid;

console.log("popup.js");

  function SubscribeNotification() {
            var userId = guid;
            //var userId = "asdasd";
			console.log(userId + " en subscribe to notification");
            $.get("http://localhost:52325/Api/Notification/subscribe?bathid=1&userId=" + userId, function (data) {

					console.log(data);

           });
        }
		function BindSuscriptionEvent()
		{
			$("#ucSubscribe").bind("click", function(){
				
				SubscribeNotification();
				
			});
			
		}
		$(function () {     
		console.log("popup.js Bind");
			BindSuscriptionEvent();
		});
		
		
		  
        $(function () {

            $.connection.hub.url = "http://localhost:52325/signalr";

            // Declare a proxy to reference the hub.
            var chat = $.connection.photonHub;
            // Create a function that the hub can call to broadcast messages.
            chat.client.broadcastMessage = function (name, message) {
                // Html encode display name and message.
                var encodedName = $('<div />').text(name).html();
                var encodedMsg = $('<div />').text(message).html();
                // Add the message to the page.
                $('#discussion').append('<li><strong>' + encodedName
                    + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
            };
            // Get the user name and store it to prepend to messages.
            $('#displayname').val("Name");
            // Set initial focus to message input box.
            $('#message').focus();
            // Start the connection.
            $.connection.hub.start().done(function () {

                chat.server.registerConId().done(function (result) {

                    console.log(result);

                    guid = result;

                });;

            });
        });
		
	
