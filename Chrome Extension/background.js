 //var HUB_BASE_URL = "http://kkcloud.azurewebsites.net/signalr";
  var HUB_BASE_URL = "http://localhost:52325/signalr";

  //var API_BASE_URL = "http://kkcloud.azurewebsites.net";
 var API_BASE_URL = "http://localhost:52325";
  
 //Method A) Using Long Lived Connections
 var frontEndProxy;
 chrome.extension.onConnect.addListener(function(port) {

   frontEndProxy = port;
   //EXAMPLE
   //port.onMessage.addListener(function(msg) {
   // port.postMessage("Hi Popup.js");
   //});
 });

 // Variables used to show 2 times the keepFirstPosition notification (the max configurable duration is 25 secs)
 var pendingNotifications = []; // IDs of the bathrooms corresponding to the notifications that the user has not yet confirmed (clicking yes or no)
 var notificationsCount = [0, 0, 0]; // times that the keepFirstPosition notification was sent for each bathroom
 
 //Method B)  Direct Manipulation of DOM
 //EXAMPLE
 //var views = chrome.extension.getViews({type: "popup"});
 //for (var i = 0; i < views.length; i++) {
 //          views[i].document.getElementById('discussion').innerHTML="My Custom Value";
 //}

 //Show system notification => BACKGROUND
 function ShowNotification(notification) {
   var opt = {
     type: "basic",
     title: notification.Title,
     message: notification.Message,
     iconUrl: "images/logo/logo48.png"
   }
   chrome.notifications.create("", opt, function() {});
 }
 
  function ShowNotificationWithButton(notification) {
   var opt = {
     type: "basic",
     title: notification.Title,
     message: notification.Message,
	 iconUrl: "images/logo/logo48.png",
	 priority: 1,
	 buttons: [
		{ title: 'No fui yo, \u00a1dejame en la fila!' },
		{ title: 'S\u00ed, fui yo' }
	  ]
   }
   var bathId = notification.Bathroom.ID;
   chrome.notifications.create(bathId + "", opt, function() {});
   
   if(pendingNotifications.indexOf(bathId) == -1){
		pendingNotifications.push(bathId);
    }
	
	setTimeout(function(){
		notificationsCount[bathId - 1]++;
		console.log('a');
		if(pendingNotifications.indexOf(bathId) != -1){
			console.log('b');
			if(notificationsCount[bathId - 1] < 2){
				console.log(notification);
				ShowNotificationWithButton(notification)				
			}
			else{
				notificationsCount[bathId - 1] = 0;
				pendingNotifications.splice(pendingNotifications.indexOf(bathId), 1)
			}
		}
		else{
			notificationsCount[bathId - 1] = 0;
		}
	}, 26000);
 }
 
 chrome.notifications.onButtonClicked.addListener(function(bathId, btnIdx) {
	if (btnIdx === 0) {		
		$.post(API_BASE_URL + "/Api/Notification/KeepFirstPosition?bathId=" + bathId)
	}
	chrome.notifications.clear(bathId);
	pendingNotifications.splice(pendingNotifications.indexOf(bathId), 1)
});

 $(function() {

   $.connection.hub.url = HUB_BASE_URL;
   

   // Declare a proxy to reference the hub.
   var chat = $.connection.photonHub;
   // Create a function that the hub can call to broadcast messages.
   chat.client.sendMessage = function(notification) {
     // Html encode display name and message.
     //var encodedName = $('<div />').text(name).html();
     //var encodedMsg = $('<div />').text(message).html();
     // Add the message to the page.
     //Method A)
     //try {
    //   var views = chrome.extension.getViews({
    //     type: "popup"
    //   });
    //   views[0].document.getElementById('discussion').innerHTML += '<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>';
    // } catch (ex) {
       //No VIEW available
    // }
     //Method B)
     //frontEndProxy.postMessage("From background " + name + " " + message);

    // frontEndProxy.postMessage(data);
	
	if(notification.Type == 1){

		ShowNotificationWithButton(notification);
	 
	 }
	 else{
		ShowNotification(notification);
	 }

   };

   // Start the connection.
   $.connection.hub.start().done(function() {
     //Register connection ID => User ID
     chat.server.registerConId().done(function(result) {
       //Save Connection ID to Chrome Local Storage
       localStorage.Guid = result;
     });;
   });

 });
