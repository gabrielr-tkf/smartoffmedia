using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;

namespace Photon.WebAPI.Classes
{
    public class PhotonHub : Hub
    {
        public static void SendMessage(string user, string bathid, string state)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PhotonHub>();
            //hubContext.Clients.All.broadcastMessage(bathid, state);
            hubContext.Clients.Client(htUsers_ConIds[user].ToString()).broadcastMessage(bathid, state);
        }
      
        public static Hashtable htUsers_ConIds = new Hashtable(20);
        public string registerConId()
        {

            string userID = Guid.NewGuid().ToString();

            bool alreadyExists = false;
            if (htUsers_ConIds.Count == 0)
            {
                htUsers_ConIds.Add(userID, Context.ConnectionId);
            }
            else
            {
                foreach (string key in htUsers_ConIds.Keys)
                {
                    if (key == userID)
                    {
                        htUsers_ConIds[key] = Context.ConnectionId;
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    htUsers_ConIds.Add(userID, Context.ConnectionId);
                }
            }

            return userID;
        }
    }
}