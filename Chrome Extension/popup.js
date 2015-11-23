// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

var API_BASE_URL = "http://kkcloud.azurewebsites.net";

// Bind button event to recieve notifications
$(function() {
  $("#ucSubscribe").bind("click", function() {
    var userId = localStorage.Guid;
    $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=1&userId=" + userId, function(data) {

			if(data.Status == "200")
			{
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			}else {
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
			}
    });
  });
});
//Show system notification => FRONTEND
function ShowNotification(title, message) {
  var opt = {
    type: "basic",
    title: title,
    message: message,
    iconUrl: "icon.png"
  }
  chrome.notifications.create("", opt, function() {});
}

//Communication with background.js
var port = chrome.extension.connect({
  name: "Sample Communication"
});
port.onMessage.addListener(function(msg) {

  	console.log("message recieved " + msg);

});
