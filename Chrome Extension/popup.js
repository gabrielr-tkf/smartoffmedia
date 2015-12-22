// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
//var API_BASE_URL = "http://kkcloud.azurewebsites.net";
var API_BASE_URL = "http://localhost:52325/";




 //Switch enable/disable notification
 var wcMensCheckbox;
 var wcWomensCheckbox;
 var wcMixedCheckbox;

//Switchery variables
 var swcMensCheckbox; 
 var swcWomensCheckbox;
 var swcMixedCheckbox; 

$(function() {

function GetBathStatus()
{
  //Get Status of all bathrooms on Load
  $.get(API_BASE_URL + "/Api/Bath/GetStatus", function(data) {

    //CSS classes = busy / free / uncertain
    //bath1, bath2, bath3
    //Bath 1
	console.log("data.BathStatusList[0].Bathroom.IsOccupied = " + data.BathStatusList[0].Bathroom.IsOccupied)
    if (data.BathStatusList[0].Bathroom.IsOccupied == true) {
		console.log("BUSY");
		$("#bath1").removeClass("free");
      $("#bath1").addClass("busy");
    } else {
		console.log("FREE");
		$("#bath1").removeClass("busy");
      $("#bath1").addClass("free");
    }
    //Bath 2
    if (data.BathStatusList[1].Bathroom.IsOccupied == true) {
		$("#bath2").removeClass("free");
      $("#bath2").addClass("busy");
    } else {
	$("#bath2").removeClass("busy");
      $("#bath2").addClass("free");
    }
    //Bath 3
    if (data.BathStatusList[2].Bathroom.IsOccupied == true) {
		$("#bath3").removeClass("free");
      $("#bath3").addClass("busy");
    } else {
		$("#bath3").removeClass("busy");
      $("#bath3").addClass("free");
    }

    console.log(data.BathStatusList[0].Bathroom.IsOccupied);
    console.log(data.BathStatusList);

  });
}
function GetNotificationStatus()
{
  var userId = localStorage.Guid;
  $.get(API_BASE_URL + "/Api/Notification/GetNotificationStatusByUser?userId=" + userId, function(data) {

    console.log("==> " + data.SubscribedNotificationBath1);
   
      $("#wcMensCheckbox").prop('checked', data.SubscribedNotificationBath1);
    
		//swcMensCheckbox.handleOnchange(false);
	
	
      $("#wcWomensCheckbox").prop('checked', data.SubscribedNotificationBath2);
   
    
      $("#wcMixedCheckbox").prop('checked', data.SubscribedNotificationBath3);
    
    console.log(data);

  });
}

	setInterval(function(){GetBathStatus();}, 3000);
	
  
   //Switch enable/disable notification
     wcMensCheckbox = document.getElementById('wcMensCheckbox');
     wcWomensCheckbox = document.getElementById('wcWomensCheckbox');
     wcMixedCheckbox = document.getElementById('wcMixedCheckbox');
  
   var checkboxOptions = {
      size: 'small',
      color: '#05820b',
      secondaryColor: '#c2000d'
    };
	try{
		 swcMensCheckbox = new Switchery(wcMensCheckbox, checkboxOptions);
		 swcWomensCheckbox = new Switchery(wcWomensCheckbox, checkboxOptions);
		 swcMixedCheckbox = new Switchery(wcMixedCheckbox, checkboxOptions);
	}catch(e){}
  //Get Status of User Notifications (On/off for all bathrooms)
  //TODO
  //setInterval(function(){GetNotificationStatus();}, 1000);
  
	GetNotificationStatus();
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
console.log("Subscribe");
        //Success
        if (data.Status == "200") {
			console.log("ok");
          ShowNotification(data.NotificationTitle, data.NotificationMessage);
          wasCallCallback1 = false;
        } else if (data.Status == "304") {
			console.log("Ñoba vacio");
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
