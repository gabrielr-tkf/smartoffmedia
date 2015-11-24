 var HUB_BASE_URL = "http://localhost:52325/signalr";

 //Method A) Using Long Lived Connections
 var frontEndProxy;
 chrome.extension.onConnect.addListener(function(port) {

   frontEndProxy = port;
   //EXAMPLE
   //port.onMessage.addListener(function(msg) {
   // port.postMessage("Hi Popup.js");
   //});
 });

 //Method B)  Direct Manipulation of DOM
 //EXAMPLE
 //var views = chrome.extension.getViews({type: "popup"});
 //for (var i = 0; i < views.length; i++) {
 //          views[i].document.getElementById('discussion').innerHTML="My Custom Value";
 //}

 //Show system notification => BACKGROUND
 function ShowNotification(title, message) {
   var opt = {
     type: "basic",
     title: title,
     message: message,
     iconUrl: "images/logo/logo48.png"
   }
   chrome.notifications.create("", opt, function() {});
 }

 $(function() {

   $.connection.hub.url = HUB_BASE_URL;

   // Declare a proxy to reference the hub.
   var chat = $.connection.photonHub;
   // Create a function that the hub can call to broadcast messages.
   chat.client.sendMessage = function(bathStatus) {
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
    var data = {
      type : 200,
      BathId : bathStatus.BathId,
      IsOccupied : bathStatus.IsOccupied
    }

    frontEndProxy.postMessage(data);

     //ShowNotification(title, message);
     ShowNotification(bathStatus.Title, bathStatus.Message);

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
