using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Photon.WebAPI.Classes;
using Photon.WebAPI.Entities;
using System.Web.Http.Cors;

namespace Photon.WebAPI.Controllers
{
    public class NotificationController : ApiController
    {

        private static Queue<string> queueNotifications = new Queue<string>();

        //TODO: Allow only used origin
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationSubscribeResponse Subscribe(string bathId, string userId)
        {

            queueNotifications.Enqueue(userId);

            NotificationSubscribeResponse response = new NotificationSubscribeResponse()
            {
                BathId = bathId,
                UserId = userId,
                Message = "User " + userId + " subscribed to bath " + bathId,
                Status = "200",
                NotificationTitle = "kkcloud",
                NotificationMessage = "Te avisaremos cuando el baño " + bathId + " este libre ;-)"
            };

            return response;

        }
        public void Publish(string bathId, bool isOccupied)
        {


            if (queueNotifications.Count > 0)
            {

                string userId = queueNotifications.Dequeue();


                PhotonHub.SendMessage(userId, "Hey!", "El baño " + bathId.ToString() + " está " + isOccupied.ToString());
            }


        }

    }
}
