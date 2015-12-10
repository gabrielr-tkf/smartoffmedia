using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using Photon.Entities;
using Photon.WebAPI.Utilities;

namespace Photon.WebAPI.Classes
{
    public class PhotonHub : Hub
    {
        //public static void SendMessage(string user, BathStatus bathStatus)
        public static void SendMessage(Notification notification)
        {
            List<User> UsersList = CacheManager.Get(Constants.UsersList) as List<User>;  

             var hubContext = GlobalHost.ConnectionManager.GetHubContext<PhotonHub>();

            //hubContext.Clients.All.broadcastMessage(bathid, state);
            hubContext.Clients.Client(UsersList.Find(a => a.ID == notification.User.ID).ID).sendMessage(notification);

            // TODO: These 2 lines are only for debugging purposes. They have to be removed
            string stringNotification = notification.User.ID.Split('-')[0] + ": " + notification.Message;
            ((List<string>)System.Web.HttpRuntime.Cache["Notifications"]).Add(stringNotification);
        }
      
        //public static List<string> UsersConnectionIds = new List<string>();
        public static List<User> UsersList = new List<User>();
        public string registerConId()
        {
            List<User> UsersList =  CacheManager.Get(Constants.UsersList) as List<User>;  
            if (UsersList.Count == 0)
            {
                UsersList.Add(new User() 
                {
                   ID = Context.ConnectionId
                });
            }
            else
            {
                if (!UsersList.Exists(a => a.ID == Context.ConnectionId))
              {
                  UsersList.Add(new User()
                  {
                      ID = Context.ConnectionId
                  });
              }
            }
            return Context.ConnectionId;
        }
    }
}