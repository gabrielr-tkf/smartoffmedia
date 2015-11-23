// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.




console.log("popup.js");
  function SubscribeNotification() {
            var userId = localStorage.Guid;
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
		
		
		function callbackNotification()
		{
			console.log("notification callback");
		}
	
		  
		  var port = chrome.extension.connect({name: "Sample Communication"});
//port.postMessage("Hi BackGround");
port.onMessage.addListener(function(msg) {
        console.log("message recieved"+ msg);
		
		if(true)
		{
			var opt = {
			  type: "basic",
			  title: "Primary Title",
			  message: "Primary message to display",
			  iconUrl: "url_to_small_icon"
			}
			chrome.notifications.create("1", opt,  callbackNotification)
		}
});
		
		
		
	
