using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using Photon.Entities;

namespace Photon.WebAPI.Classes
{
    public class PhotonHub : Hub
    {
        public static void SendMessage(string user, BathStatus bathStatus)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PhotonHub>();
            //hubContext.Clients.All.broadcastMessage(bathid, state);
            hubContext.Clients.Client(UsersConnectionIds.Find(a=> a == user)).sendMessage(bathStatus);
        }
      
        public static List<string> UsersConnectionIds = new List<string>();
        public string registerConId()
        {
            if (UsersConnectionIds.Count == 0)
            {
                UsersConnectionIds.Add(Context.ConnectionId);
            }
            else
            {
              if(!UsersConnectionIds.Exists(a=> a == Context.ConnectionId))
              {
                  UsersConnectionIds.Add(Context.ConnectionId);
              }
            }
            return Context.ConnectionId;
        }
    }
}