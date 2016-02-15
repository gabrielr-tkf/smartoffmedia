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
 
 function GetBathStatus()
{
	//Get Status of all bathrooms on Load
  $.get(API_BASE_URL + "/Api/Bath/GetStatus", function(data) {

	var userId = localStorage.Guid;  
  
    //CSS classes = busy / free / uncertain
    //bath1, bath2, bath3
    //Bath 1
	var panel = $("#bath1 .panel");
		
	if(data.BathStatusList[0].Bathroom.PhotonDevice.Connected == true)
	{
		$("#bath1-error").hide();
		$("#bath1 .notificationRow").show();
		$("#bath1").removeClass('uncertain');
		$("#bath1 .btnReservar").show()
		
		var usersLine = data.BathStatusList[0].UsersLine;
		var userPosition = findWithAttr(usersLine, 'ID', userId) + 1;
				
		if (data.BathStatusList[0].Bathroom.IsOccupied == true) {
			$("#bath1").removeClass("free");
			$("#bath1").removeClass("your-turn");
			$("#bath1").addClass("busy");
			$("#bath1 .state").text("Ocupado");
			$("#bath1 .linePosition span").hide();
			
			if(usersLine.length > 0 && userPosition != -1 && !isNaN(userPosition)){
				$("#bath1 .linePosition span").show();
				$("#bath1 .linePosition span").text(userPosition);
				
				if(!panel.hasClass("flip")){
					panel.addClass('flip');
				}
			}
			else{
				if(panel.hasClass("flip")){
					panel.removeClass('flip');
				}
			}
		} else {
			if(usersLine.length > 0){
				if(usersLine[0].ID == userId){
					$("#bath1").removeClass("free");
					$("#bath1").removeClass("busy");
					$("#bath1").addClass("your-turn");
					$("#bath1 .state").text("¡Te toca!");
					$("#bath1 .btnReservar").hide();
					
					if(panel.hasClass("flip")){
						panel.removeClass('flip');
					}
				}
				else{
					$("#bath1").removeClass("free");
					$("#bath1").removeClass("your-turn");
					$("#bath1").addClass("busy");
					$("#bath1 .state").text("Reservado");
					
					if(userPosition != -1 && !isNaN(userPosition)){
						$("#bath1 .linePosition span").show();
						$("#bath1 .linePosition span").text(userPosition);
						
						if(!panel.hasClass("flip")){
							panel.addClass('flip');
						}
					}
					else{
						if(panel.hasClass("flip")){
							panel.removeClass('flip');
						}
					}
				}
			}
			else{
				$("#bath1").removeClass("busy");
				$("#bath1").removeClass("your-turn");
				$("#bath1").addClass("free");
				$("#bath1 .state").text("Libre");
				
				if(panel.hasClass("flip")){
					panel.removeClass('flip');
				}
			}
		}
	}else{
		$("#bath1-error").show();
		$("#bath1 .notificationRow").hide();
		$("#bath1").addClass('uncertain');
	}
	
   
    //Bath 2
	if(data.BathStatusList[1].Bathroom.PhotonDevice.Connected == true)
	{
		$("#bath2-error").hide();
		$("#bath2 .notificationRow").show();
		$("#bath2").removeClass('uncertain');
		
		if (data.BathStatusList[1].Bathroom.IsOccupied == true) {
			$("#bath2").removeClass("free");
			$("#bath2").removeClass("your-turn");
			$("#bath2").addClass("busy");
			$("#bath2 .state").text("Ocupado");
		} else {
			if(data.BathStatusList[1].UsersLine.length > 0){
				if(data.BathStatusList[1].UsersLine[0].ID == userId){
					$("#bath2").removeClass("free");
					$("#bath2").removeClass("busy");
					$("#bath2").addClass("your-turn");
					$("#bath2 .state").text("¡Te toca!");
				}
				else{
					$("#bath2").removeClass("free");
					$("#bath2").removeClass("your-turn");
					$("#bath2").addClass("busy");
					$("#bath2 .state").text("Reservado");
				}
			}
			else{
				$("#bath2").removeClass("busy");
				$("#bath2").removeClass("your-turn");
				$("#bath2").addClass("free");
				$("#bath2 .state").text("Libre");
			}
		}
	}else{
		$("#bath2-error").show();
		$("#bath2 .notificationRow").hide();
		$("#bath2").addClass('uncertain');
	}
	
	//Bath 3
	if(data.BathStatusList[2].Bathroom.PhotonDevice.Connected == true)
	{
		$("#bath3-error").hide();
		$("#bath3 .notificationRow").show();
		$("#bath3").removeClass('uncertain');
		
		if (data.BathStatusList[2].Bathroom.IsOccupied == true) {
			$("#bath3").removeClass("free");
			$("#bath3").removeClass("your-turn");
			$("#bath3").addClass("busy");
			$("#bath3 .state").text("Ocupado");
		} else {
			if(data.BathStatusList[2].UsersLine.length > 0){
				if(data.BathStatusList[2].UsersLine[0].ID == userId){
					$("#bath3").removeClass("free");
					$("#bath3").removeClass("busy");
					$("#bath3").addClass("your-turn");
					$("#bath3 .state").text("¡Te toca!");
				}
				else{
					$("#bath3").removeClass("free");
					$("#bath3").removeClass("your-turn");
					$("#bath3").addClass("busy");
					$("#bath3 .state").text("Reservado");
				}
			}
			else{
				$("#bath3").removeClass("busy");
				$("#bath3").removeClass("your-turn");
				$("#bath3").addClass("free");
				$("#bath3 .state").text("Libre");
			}
		}
	}else{
		$("#bath3-error").show();
		$("#bath3 .notificationRow").hide();
		$("#bath3").addClass('uncertain');
	}

	
  });
}

function GetNotificationStatus()
{
  var userId = localStorage.Guid;
  $.get(API_BASE_URL + "/Api/Notification/GetNotificationStatusByUser?userId=" + userId, function(data) {
   
		$("#bath1 .front .actionBtn input").val(data.SubscribedNotificationBath1 ? "Cancelar" : "Reservar")
		executeChangeBath1 = false;
		swcMensCheckbox.handleOnchange(data.SubscribedNotificationBath1);
		
	
		$("#bath2 .front .actionBtn input").val(data.SubscribedNotificationBath1 ? "Cancelar" : "Reservar")
		executeChangeBath2 = false;
		swcWomensCheckbox.handleOnchange(data.SubscribedNotificationBath2);
   
    
		$("#bath3 .front .actionBtn input").val(data.SubscribedNotificationBath1 ? "Cancelar" : "Reservar")
		executeChangeBath3 = false;
		swcMixedCheckbox.handleOnchange(data.SubscribedNotificationBath3);
    
		executeChangeBath1 = true;
		executeChangeBath2 = true;
		executeChangeBath3 = true;
		

  });
}

$(function() {

	setTimeout(function(){
		var svg = new Walkway({
		  selector: '#boy',
		  duration: 500,
		  easing: 'linear'
		});
		var svg2 = new Walkway({
		  selector: '#girl',
		  duration: 500,
		  easing: 'linear'
		});
		var svg3 = new Walkway({
		  selector: '#boy2',
		  duration: 500,
		  easing: 'linear'
		});
		
		$('#loader-wrapper').fadeOut(function() {
			svg.draw();
			svg2.draw();
			svg3.draw();
		});
		$('#container').css("visibility", "");
	},
	3000);
	
	$('.panel .btn').click(function(e){
		
		e.preventDefault();  
		var btn = $(this);
		var panel = $(this).closest('.panel');
		var flip = panel.hasClass('flip');
		
		setTimeout(function(){
			
			var userId = localStorage.Guid;
			
			if($(btn).parent().hasClass("btnReservar")){
				
				$.get(API_BASE_URL + "/Api/Notification/subscribe?bathid=" + $(btn).parents(".bath").data("bath-id") + "&userId=" + userId, function(data) {
				//Success
				if (data.Status == "200") {				
				  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
				  panel.addClass('flip');
				} else if (data.Status == "304") {
				  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
				} else {
				  //Error!
				  ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
				}

			  });
			} else if($(btn).parent().hasClass("btnCancelar")) {
				//UN-SUBSCRIBE MEN BATHROOM
				$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=1&userId=" + userId + "&sendMessage=false", function(data) {

				  //Success
				  if (data.Status == "200") {
					ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);					
					panel.removeClass('flip');
				  } else if (data.Status == "304") {	
					ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
				  } else {
					//Error
					ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
				  }
				});
			}
		}, 300);
		

	});
	
	Waves.init();
    Waves.attach('.float-buttons', ['waves-button', 'waves-radius', 'waves-float']);
	

	setInterval(function(){GetBathStatus();}, 3000);
	GetBathStatus();
	
  
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
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  wasCallCallback1 = false;
			} else if (data.Status == "304") {
			  wasCallCallback1 = true;
			  wcMensCheckbox.checked = !wcMensCheckbox.checked;
			  
			  swcMensCheckbox.handleOnchange(false);
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			} else {
			  //Error!
			  ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
			}

		  });
		} else {
		  if (!wasCallCallback1) {
			//UN-SUBSCRIBE MEN BATHROOM
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=1&userId=" + userId + "&sendMessage=false", function(data) {

			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  } else if (data.Status == "304") {
				//Keep button green and show error message
				wcMensCheckbox.checked = true;
				wasCallCallback1 = true;				
				swcMensCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
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
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  wasCallCallback2 = false;
			} else if (data.Status == "304") {
			  wasCallCallback2 = true;
			  wcWomensCheckbox.checked = !wcWomensCheckbox.checked;
			  swcWomensCheckbox.handleOnchange(false);
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			} else {
			  //Subscription Error!
			  ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
			}
		  });
		} else {

		  if (!wasCallCallback2) {
			//UN-SUBSCRIBE WOMAN BATHROOM
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=2&userId=" + userId + "&sendMessage=false", function(data) {
			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  } else if (data.Status == "304") {
				//Keep button green and show error message
				swcWomensCheckbox.checked = true;
				wasCallCallback2 = true;				
				swcWomensCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
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
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  wasCallCallback3 = false;
			} else if (data.Status == "304") {
			  wasCallCallback3 = true;
			  wcMixedCheckbox.checked = !wcMixedCheckbox.checked;
			  swcMixedCheckbox.handleOnchange(false);
			  ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			} else {
			  //Error!
			  ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
			}

		  });
		} else {

		  if (!wasCallCallback3) {
			//UN-SUBSCRIBE MIX BATHROOM
			$.get(API_BASE_URL + "/Api/Notification/unsubscribe?bathid=3&userId=" + userId + "&sendMessage=false", function(data) {
			  //Success
			  if (data.Status == "200") {
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);
			  
			  
			  } else if (data.Status == "304") {

				//Keep button green and show error message
				swcMixedCheckbox.checked = true;
				wasCallCallback3 = true;	
				swcMixedCheckbox.handleOnchange(true);
				ShowNotification(data.NotificationTitle, data.NotificationMessage, data.AudioFile);

			  } else {
				//Error
				ShowNotification("Error :-(", "Por favor intentá nuevamente en unos minutos.", "");
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
function ShowNotification(title, message, audioFile) {
  var opt = {
    type: "basic",
    title: title,
    message: message,
    iconUrl: "images/logo/logo48.png"
  }
  chrome.notifications.create("", opt, function() {});
  
  if(audioFile != ""){
	var audio = new Audio(audioFile);
	audio.play();
  }
}

//Communication with background.js
var port = chrome.extension.connect({
  name: "Sample Communication"
});

port.postMessage("ValidateConnection");

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

function findWithAttr(array, attr, value) {
    for(var i = 0; i < array.length; i += 1) {
        if(array[i][attr] === value) {
            return i;
        }
    }
}
