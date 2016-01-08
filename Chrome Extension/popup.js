// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
var API_BASE_URL = "http://kkcloud.azurewebsites.net";
// var API_BASE_URL = "http://localhost:52325/";




 //Switch enable/disable notification
 var wcMensCheckbox;
 var wcWomensCheckbox;
 var wcMixedCheckbox;

//Switchery variables
 var swcMensCheckbox; 
 var swcWomensCheckbox;
 var swcMixedCheckbox; 

 var executeChangeBath1 = true;
 var executeChangeBath2 = true;
 var executeChangeBath3 = true;
 
$(function() {

function GetBathStatus()
{
	//Get Status of all bathrooms on Load
  $.get(API_BASE_URL + "/Api/Bath/GetStatus", function(data) {

    //CSS classes = busy / free / uncertain
    //bath1, bath2, bath3
    //Bath 1
    if (data.BathStatusList[0].Bathroom.IsOccupied == true) {
		$("#bath1").removeClass("free");
      $("#bath1").addClass("busy");
    } else {
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
	
	$('#loader-wrapper').fadeOut();
	$('#container').css("visibility", "");

  });
}
function GetNotificationStatus()
{
  var userId = localStorage.Guid;
  $.get(API_BASE_URL + "/Api/Notification/GetNotificationStatusByUser?userId=" + userId, function(data) {
   
		$("#wcMensCheckbox").prop('checked', data.SubscribedNotificationBath1);
		executeChangeBath1 = false;
		swcMensCheckbox.handleOnchange(data.SubscribedNotificationBath1);
		
	
		$("#wcWomensCheckbox").prop('checked', data.SubscribedNotificationBath2);
		executeChangeBath2 = false;
		swcWomensCheckbox.handleOnchange(data.SubscribedNotificationBath2);
   
    
		$("#wcMixedCheckbox").prop('checked', data.SubscribedNotificationBath3);
		executeChangeBath3 = false;
		swcMixedCheckbox.handleOnchange(data.SubscribedNotificationBath3);
    
		executeChangeBath1 = true;
		executeChangeBath2 = true;
		executeChangeBath3 = true;
		

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
  setInterval(function(){GetNotificationStatus();}, 3000);
  
	//GetNotificationStatus();

  //Bathroom 1
  var wasCallCallback1 = false;

  $(wcMensCheckbox).on('change', function() {
  
	if(executeChangeBath1){
  
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
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=1&userId=" + userId + "&sendMessage=false", function(data) {

			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			  } else if (data.Status == "304") {
				//Keep button green and show error message
				wcMensCheckbox.checked = true;
				wasCallCallback1 = true;				
				swcMensCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
			  }
			});
		  }
		}
	}
	else{
		executeChangeBath1 = true;
	}
  });
  //Bathroom 2
  var wasCallCallback2 = false;
  $(wcWomensCheckbox).on('change', function() {

	if(executeChangeBath2){
  
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
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=2&userId=" + userId + "&sendMessage=false", function(data) {
			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			  } else if (data.Status == "304") {
				//Keep button green and show error message
				swcWomensCheckbox.checked = true;
				wasCallCallback2 = true;				
				swcWomensCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
			  }

			});
		  }
		}
	}
	else{
		executeChangeBath2 = true;
	}
  });
  //Bathroom 3
  var wasCallCallback3 = false;
  $(wcMixedCheckbox).on('change', function() {
  
	if(executeChangeBath3){
  
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
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=3&userId=" + userId + "&sendMessage=false", function(data) {
			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage);
			  
			  
			  } else if (data.Status == "304") {

				//Keep button green and show error message
				swcMixedCheckbox.checked = true;
				wasCallCallback3 = true;	
				swcMixedCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage);

			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.");
			  }
			});
		  }
		}
	}
	else{
		executeChangeBath3 = true;
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
  if (data.type == 200) {
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
