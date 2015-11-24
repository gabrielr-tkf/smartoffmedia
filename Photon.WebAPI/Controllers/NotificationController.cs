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
using System.Web;
using Photon.WebAPI.Utilities;
using Photon.Entities;

namespace Photon.WebAPI.Controllers
{
    public class NotificationController : ApiController
    {

        /// <summary>
        /// Subscribe user to notification
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //TODO: Allow only used origin
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationSubscribeResponse Subscribe(int bathId, string userId)
        {
            NotificationSubscribeResponse response = new NotificationSubscribeResponse();
            response.BathId = bathId.ToString();
            response.UserId = userId;
            response.NotificationTitle = "kkcloud";

            bool bathIsOccupied = false;
            List<string> bathQueue = (CacheManager.Get(Constants.BathQueues) as List<List<string>>)[bathId - 1];

            bathIsOccupied = (CacheManager.Get(Constants.OccupiedBaths) as List<bool>)[bathId - 1];


            bool queueIsEmpty = (CacheManager.Get(Constants.BathQueues) as List<List<string>>)[bathId - 1].Count == 0;

            if (!queueIsEmpty || queueIsEmpty && bathIsOccupied)
            {
                if (bathQueue.Contains(userId))
                {
                    response.Message = "User " + userId + " was already subscribed to bath " + bathId;
                    response.Status = "200";
                    response.NotificationMessage = "Ya estás en la cola para ese baño";
                }
                else
                {
                    bathQueue.Add(userId);

                    response.Message = "User " + userId + " subscribed to bath " + bathId;
                    response.Status = "200";
                    response.NotificationMessage = "Te avisamos cuando el baño " + bathId + " este libre ;-)";
                }
            }
            else
            {
                response.Message = "Bathroom " + bathId + " is free and the queue is empty";
                response.Status = "304"; //Not Modified.
                response.NotificationMessage = "El baño está libre y no hay nadie esperando, va pa i";
            }

            return response;

        }
        /// <summary>
        /// UnSubscribe user to notification
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationUnSubscribeResponse Unsubscribe(int bathId, string userId)
        {
            NotificationUnSubscribeResponse response = new NotificationUnSubscribeResponse();
            response.BathId = bathId.ToString();
            response.UserId = userId;
            response.NotificationTitle = "kkcloud";


            List<string> bathQueue = (CacheManager.Get(Constants.BathQueues) as List<List<string>>)[bathId - 1];

            bool queueIsEmpty = (CacheManager.Get(Constants.BathQueues) as List<List<string>>)[bathId - 1].Count == 0;

            if (!queueIsEmpty)
            {

                // Remove user from Queue
                bathQueue.Remove(userId);

                response.Message = "User " + userId + " removed from list to bath " + bathId;
                response.Status = "200";
                response.NotificationMessage = "Ya no estás en la cola para ese baño";

            }
            else
            {
                response.Message = "Empty Queue => Bathroom " + bathId;
                response.Status = "200";
                response.NotificationMessage = "Ya no estabas en la cola para este baño";
            }

            return response;
        }
        /// <summary>
        ///  Send notification to Subscribed user (first in the list)
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="isOccupied"></param>
        public void Publish(int bathId, bool isOccupied)
        {
            List<string> bathQueue = (CacheManager.Get(Constants.BathQueues) as List<List<string>>)[bathId - 1];

            if (bathQueue.Count > 0)
            {
                //Get first user from Queue
                string userId = bathQueue.First();

                //Remove first item from Queue
                bathQueue.RemoveAt(0);

                BathStatus bathStatus = new BathStatus()
                {
                    Title = "Hey!",
                    Message = "El baño " + bathId.ToString() + " está " + isOccupied.ToString(),
                    BathId = bathId,
                    IsOccupied = isOccupied
                };
                PhotonHub.SendMessage(userId, bathStatus);
            }


        }

    }
}
