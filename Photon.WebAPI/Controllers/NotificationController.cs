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
            List<string> bathLine = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1];

            bathIsOccupied = (CacheManager.Get(Constants.OccupiedBaths) as List<bool>)[bathId - 1];

            bool lineIsEmpty = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1].Count == 0;

            if (!lineIsEmpty || lineIsEmpty && bathIsOccupied)
            {
                if (bathLine.Contains(userId))
                {
                    response.Message = "User " + userId + " was already subscribed to bath " + bathId;
                    response.Status = "200";
                    response.NotificationMessage = "Ya estás en la cola para ese baño";
                }
                else
                {
                    bathLine.Add(userId);

                    response.Message = "User " + userId + " subscribed to bath " + bathId;
                    response.Status = "200";
                    response.NotificationMessage = "Te avisamos cuando el baño " + bathId + " este libre ;-)";
                }
            }
            else
            {
                response.Message = "Bathroom " + bathId + " is free and the line is empty";
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


            List<string> bathLine = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1];

            bool lineIsEmpty = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1].Count == 0;

            if (!bathLine.Contains(userId))
            {

                // Remove user from the line
                bathLine.Remove(userId);

                response.Message = "User " + userId + " removed from list to bath " + bathId;
                response.Status = "200";
                response.NotificationMessage = "Ya no estás en la cola para ese baño";

            }
            else
            {
                response.Message = "Empty line => Bathroom " + bathId;
                response.Status = "200";
                response.NotificationMessage = "Ya no estabas en la cola para este baño";
            }

            return response;
        }

        /// <summary>
        /// Send notification to subscribed users
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="isOccupied"></param>
        public void Publish(int bathId, bool isOccupied)
        {
            List<string> bathLine = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1];

            if (bathLine.Count > 0)
            {
                // If the bath was freed notify all the users in the line
                if (!isOccupied)
                {
                    (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[bathId - 1] = true;

                    for (int i = 0; i < bathLine.Count; i++)
                    {
                        string userId = bathLine[i];

                        // If the user was first in line for the bath, send him/her a notification
                        if (i == 0)
                        {
                            BathStatus bathStatus = new BathStatus()
                            {
                                Title = "Hey!",
                                Message = "¡El baño " + bathId.ToString() + " está libre y es tu turno!",
                                BathId = bathId,
                                IsOccupied = isOccupied
                            };
                            PhotonHub.SendMessage(userId, bathStatus);
                        }
                        // Send an advance notification to the rest of the users in the line
                        else
                        {
                            BathStatus bathStatus = new BathStatus()
                            {
                                Title = "Hey!",
                                Message = "El baño " + bathId.ToString() + " está libre y sos el " + (i + 1) + "º en la fila",
                                BathId = bathId,
                                IsOccupied = isOccupied
                            };
                            PhotonHub.SendMessage(userId, bathStatus);
                        }
                    }
                }
                // If the bath was occupied, ask the first user in line if he is in there, in order to prevent
                // another user from taking his/her place, making him/her go to the end of the line again
                else
                {
                    string userId = bathLine.First();

                    BathStatus bathStatus = new BathStatus()
                    {
                        Title = "Hey!",
                        Message = "El baño " + bathId.ToString() + " fue ocupado y tenías el primer lugar, ¿fuiste vos?",
                        BathId = bathId,
                        IsOccupied = isOccupied
                    };
                    PhotonHub.SendMessage(userId, bathStatus);
                }
            }            
        }

        /// <summary>
        /// Make the bath line advance and notify the users about their new position.
        /// This method should be called when the first user in line doesn't indicate that he/she is,
        /// not using the bath, after a period of 45 seconds, or when the bath stays free for 60 seconds
        /// (in which case the first user in line is removed from it)
        /// /// <param name="bathId">The id of the bathroom</param>
        /// /// <param name="removeFromAll">Remove the user from all the bathroom lines</param>
        /// </summary>
        public void AdvanceLine(int bathId, bool removeFromAll)
        {
            List<string> bathLine = (CacheManager.Get(Constants.BathLines) as List<List<string>>)[bathId - 1];

            if (bathLine.Count > 0)
            {
                string userId = bathLine.First();

                if (removeFromAll)
                {
                    Unsubscribe(1, userId);
                    Unsubscribe(2, userId);
                    Unsubscribe(3, userId);
                }
                else
                {
                    Unsubscribe(bathId, userId);
                }
            }

            (CacheManager.Get(Constants.LastLineAdvanceTimes) as List<DateTime>)[bathId] = DateTime.Now;
        }

        /// <summary>
        /// Method called when the first user in line indicates that he/she was not the one occupying the bath
        /// </summary>
        /// <param name="bathId"></param>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public void KeepFirstPosition(int bathId){
            (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[bathId - 1] = false;
        }

    }
}
