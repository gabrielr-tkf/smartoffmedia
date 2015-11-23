  var guid;
  
  

	console.log("background");

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

				console.log("background start");
                chat.server.registerConId().done(function (result) {

				
				console.log("background registerConId");
                    console.log(result);
					
					

                    guid = result;
					
					localStorage.Guid = guid;
					
					
					
					console.log("registerConId Bind " + guid);
						BindSuscriptionEvent();

					});;

            });
        });