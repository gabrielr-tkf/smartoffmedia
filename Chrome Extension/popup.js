// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
//var API_BASE_URL = "http://kkcloud.azurewebsites.net";
var API_BASE_URL = "http://localhost:52325/";



//var setCheckStatusOnLoad = false;

$(function() {

  //Get Status of all bathrooms on Load
  $.get(API_BASE_URL + "/Api/Bath/GetStatus", function(data) {

    //CSS classes = busy / free / uncertain
    //bath1, bath2, bath3
    //Bath 1
    if (data.BathStatusList[0].Bathroom.IsOccupied == "true") {
      $("#bath1").addClass("busy");
    } else {
      $("#bath1").addClass("free");
    }
    //Bath 2
    if (data.BathStatusList[1].Bathroom.IsOccupied == "true") {
      $("#bath2").addClass("busy");
    } else {
      $("#bath2").addClass("free");
    }
    //Bath 3
    if (data.BathStatusList[2].Bathroom.IsOccupied == "true") {
      $("#bath3").addClass("busy");
    } else {
      $("#bath3").addClass("free");
    }

    console.log(data.BathStatusList[0].Bathroom.IsOccupied);
    console.log(data.BathStatusList);

  });
  var userId = localStorage.Guid;
  $.get(API_BASE_URL + "/Api/Notification/GetNotificationStatusByUser?userId=" + userId, function(data) {

    console.log("==> " + data.SubscribedNotificationBath1);
    if (data.SubscribedNotificationBath1 == true) {
      console.log("Entro true");
      $("#wcMensCheckbox").prop('checked', true);
    }
    if (data.SubscribedNotificationBath2 == "true") {
      $("#wcWomensCheckbox").prop('checked', true);
    }
    if (data.SubscribedNotificationBath3 == "true") {
      $("#wcMixedCheckbox").prop('checked', true);
    }
    console.log(data);

    //Switch enable/disable notification
    var wcMensCheckbox = document.getElementById('wcMensCheckbox');
    var wcWomensCheckbox = document.getElementById('wcWomensCheckbox');
    var wcMixedCheckbox = document.getElementById('wcMixedCheckbox');

    var checkboxOptions = {
      size: 'small',
      color: '#05820b',
      secondaryColor: '#c2000d'
    };

    var swcMensCheckbox = new Switchery(wcMensCheckbox, checkboxOptions);
    var swcWomensCheckbox = new Switchery(wcWomensCheckbox, checkboxOptions);
    var swcMixedCheckbox = new Switchery(wcMixedCheckbox, checkboxOptions);

  });

  //Get Status of User Notifications (On/off for all bathrooms)
  //TODO
});

// Bind button event to recieve notifications
$(function() {


  /*
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
    */

  //Bathroom 1
  var wasCallCallback1 = false;

  $(wcMensCheckbox).on('change', function() {

    var userId = localStorage.Guid;

    if (JSON.parse($(this)[0].checked)) {
      //SUBSCRIBE MEN BATHROOM
      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=1&userId=" + userId, function(data) {

        //Success
        if (data.Status == "200") {
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
          wasCallCallback1 = false;
        } else if (data.Status == "304") {
          wasCallCallback1 = true;
          wcMensCheckbox.checked = !wcMensCheckbox.checked;
          swcMensCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
        } else {
          //Error!
          ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
        }

      });
    } else {

      if (!wasCallCallback1) {
        //UN-SUBSCRIBE MEN BATHROOM
        $.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=1&userId=" + userId, function(data) {

          //Success
          if (data.Status == "200") {
            ShowNotification(data.NotificationTitle, data.NotificationMessage);
          } else if (data.Status == "304") {
            //Keep button green and show error message
            wcMensCheckbox.checked = true;
            swcMensCheckbox.handleOnchange(true);
            ShowNotification(data.NotificationTitle, data.NotificationMessage);
          } else {
            //Error
            ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
          }
        });
      }
    }
  });
  //Bathroom 2
  var wasCallCallback2 = false;
  $(wcWomensCheckbox).on('change', function() {

    var userId = localStorage.Guid;

    if (JSON.parse($(this)[0].checked)) {

      //SUBSCRIBE WOMAN BATHROOM
      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=2&userId=" + userId, function(data) {
        //Subscription Success
        if (data.Status == "200") {
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
          wasCallCallback2 = false;
        } else if (data.Status == "304") {
          wasCallCallback2 = true;
          wcWomensCheckbox.checked = !wcWomensCheckbox.checked;
          swcWomensCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
        } else {
          //Subscription Error!
          ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
        }
      });
    } else {

      if (!wasCallCallback2) {
        //UN-SUBSCRIBE WOMAN BATHROOM
        $.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=2&userId=" + userId, function(data) {
          //Success
          if (data.Status == "200") {
            ShowNotification(data.NotificationTitle, data.NotificationMessage);
          } else if (data.Status == "304") {
            //Keep button green and show error message
            swcWomensCheckbox.checked = true;
            swcWomensCheckbox.handleOnchange(true);
            ShowNotification(data.NotificationTitle, data.NotificationMessage);
          } else {
            //Error
            ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
          }

        });
      }
    }
  });
  //Bathroom 3
  var wasCallCallback3 = false;
  $(wcMixedCheckbox).on('change', function() {
    //SUBSCRIBE MIX BATHROOM
    var userId = localStorage.Guid;
    if (JSON.parse($(this)[0].checked)) {

      $.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=3&userId=" + userId, function(data) {
        //Success
        if (data.Status == "200") {
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
          wasCallCallback3 = false;
        } else if (data.Status == "304") {
          wasCallCallback3 = true;
          wcMixedCheckbox.checked = !wcMixedCheckbox.checked;
          swcMixedCheckbox.handleOnchange(false);
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
        } else {
          //Error!
          ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
        }

      });
    } else {

      if (!wasCallCallback3) {
        //UN-SUBSCRIBE MIX BATHROOM
        $.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=3&userId=" + userId, function(data) {
          //Success
          if (data.Status == "200") {
            ShowNotification(data.NotificationTitle, data.NotificationMessage);
          } else if (data.Status == "304") {

            //Keep button green and show error message
            swcMixedCheckbox.checked = true;
            swcMixedCheckbox.handleOnchange(true);
            ShowNotification(data.NotificationTitle, data.NotificationMessage);

          } else {
            //Error
            ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
          }
        });
      }
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
  if (data.type == 200) {
    console.log(true);
    if (!data.IsOccupied) {
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
