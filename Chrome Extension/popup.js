// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

var API_BASE_URL = "http://localhost:52325/";

var wcMensCheckbox   = document.getElementById('wcMensCheckbox');
var wcWomensCheckbox = document.getElementById('wcWomensCheckbox');
var wcMixedCheckbox  = document.getElementById('wcMixedCheckbox');

var swcMensCheckbox = new Switchery(wcMensCheckbox, {
  size: 'small',
  color: '#05820b',
  secondaryColor: '#c2000d'
});

var swcWomensCheckbox = new Switchery(wcWomensCheckbox, {
  size: 'small',
  color: '#05820b',
  secondaryColor: '#c2000d'
});

var swcMixedCheckbox = new Switchery(wcMixedCheckbox, {
  size: 'small',
  color: '#05820b',
  secondaryColor: '#c2000d'
});

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

  var fiveSeconds = new Date().getTime() + 3000 * 60;
  $('.countdown').countdown(fiveSeconds)
  .on('update.countdown', function(event) {
    var $this = $(this);
    $this.html(event.strftime('<span>%M:%S</span>'));
  })
  .on('finish.countdown', function(event) {
    $(this).html('This offer has expired!').parent().addClass('disabled');
    // Test for notification support.
    if (window.Notification) {
      //show();
      //
    }
  });

  $(wcMensCheckbox).on('change', function() {
    if(JSON.parse($(this)[0].checked)){
      var userId = localStorage.Guid;
      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=1&userId=" + userId, function(data) {
  			if(data.Status == "200")
  			{
  				ShowNotification(data.NotificationTitle, data.NotificationMessage);
  			}else if(data.Status == "304"){
          wcMensCheckbox.checked = !wcMensCheckbox.checked;
          swcMensCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
        }else {
  				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
  			}
      });
    }else{
      //unsuscribe
    }
  });

  $(wcWomensCheckbox).on('change', function() {
    if(JSON.parse($(this)[0].checked)){
      var userId = localStorage.Guid;
      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=2&userId=" + userId, function(data) {
  			if(data.Status == "200")
  			{
        }else if(data.Status == "304"){
  				ShowNotification(data.NotificationTitle, data.NotificationMessage);
          wcWomensCheckbox.checked = !wcWomensCheckbox.checked;
          swcWomensCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
  			}else {
  				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
  			}
      });
    }else{
      //unsuscribe
    }
  });

  $(wcMixedCheckbox).on('change', function() {
    if(JSON.parse($(this)[0].checked)){
      var userId = localStorage.Guid;
      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=3&userId=" + userId, function(data) {
  			if(data.Status == "200")
  			{
  				ShowNotification(data.NotificationTitle, data.NotificationMessage);
        }else if(data.Status == "304"){
          wcMixedCheckbox.checked = !wcMixedCheckbox.checked;
          swcMixedCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
  			}else {
  				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
  			}
      });
    }else{
      //unsuscribe
    }
  });

});
//Show system notification => FRONTEND
function ShowNotification(title, message) {
  var opt = {
    type: "basic",
    title: title,
    message: message,
    iconUrl: "images/logo/logo48.png"
  }
  chrome.notifications.create("", opt, function() {});
}

//Communication with background.js
var port = chrome.extension.connect({
  name: "Sample Communication"
});
port.onMessage.addListener(function(data) {
	console.log(data);
  if(data.type == 200) {
    console.log(true);
    if(!data.IsOccupied) {
      switch (data.BathId) {
        case 1:
          wcMensCheckbox.checked = !wcMensCheckbox.checked;
          swcMensCheckbox.handleOnchange(false);
          break;
        case 2:
          wcWomensCheckbox.checked = !wcWomensCheckbox.checked;
          swcWomensCheckbox.handleOnchange(false);
          break;
        case 3:
          wcMixedCheckbox.checked = !wcMixedCheckbox.checked;
          swcMixedCheckbox.handleOnchange(false);
          break;
        default:

      }
    }
  }
});
