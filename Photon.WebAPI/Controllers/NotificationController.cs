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

            BathroomLine bathroomLine = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>).First(a => a.Bathroom.ID == bathId);

            List<User> usersInLine = bathroomLine.UsersLine;

            bathIsOccupied = bathroomLine.Bathroom.IsOccupied;

            bool lineIsEmpty = usersInLine.Count == 0;

            if (!lineIsEmpty || lineIsEmpty && bathIsOccupied)
            {
                if (usersInLine.Exists(a => a.ID == userId))
                {
                    response.Message = "Success";
                    response.Status = "200";
                    response.NotificationMessage = "Ya estás en la cola para ese baño";
                }
                else
                {
                    usersInLine.Add(new User()
                    {
                            ID = userId
                    });

                    response.Message = "Success";
                    response.Status = "200";
                    response.NotificationMessage = "Te avisamos cuando el baño " + bathId + " este libre ;-)";

                    if (usersInLine.Count == 1)
                    {
                        bathroomLine.LastTimesFirstChanged = DateTime.Now;
                    }
                }
            }
            else
            {
                response.Message = "Success";
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
        public NotificationUnsubscribeResponse Unsubscribe(int bathId, string userId)
        {
            NotificationUnsubscribeResponse response = new NotificationUnsubscribeResponse();
            response.BathId = bathId.ToString();
            response.UserId = userId;
            response.NotificationTitle = "kkcloud";


            List<User> usersInLine = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>).First(a => a.Bathroom.ID == bathId).UsersLine;

            bool lineIsEmpty = usersInLine.Count == 0;

            if (usersInLine.Exists(a => a.ID == userId))
            {

                // Remove user from the line
                usersInLine.RemoveAt(usersInLine.FindIndex(a => a.ID == userId));
               
                response.Message = "Success";
                response.Status = "200";
                response.NotificationMessage = "Ya no estás en la cola para ese baño";

            }
            else
            {
                response.Message = "Success";
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
        public void Publish(Bathroom bathroom)
        {
            List<User> usersInLine = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>).First(a=>a.Bathroom.ID == bathroom.ID).UsersLine;

            if (usersInLine.Count > 0)
            {
                // If the bath was freed notify all the users in the line
                if (!bathroom.IsOccupied)
                {
                    (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[bathroom.ID-1] = true;

                    for (int i = 0; i < usersInLine.Count; i++)
                    {
                        //string userId = bathLine[i].ID;
                        User user = usersInLine[i];

                        // If the user was first in line for the bath, send him/her a notification
                        if (i == 0)
                        {
                            //BathStatus bathStatus = new BathStatus()
                            //{
                            //    Title = "Hey!",
                            //    Message = "¡El baño " + bathroom.ID.ToString() + " está libre y es tu turno!",
                            //    BathId = bathroom.ID,
                            //    IsOccupied = bathroom.IsOccupied
                            //};

                            Notification notification = new Notification()
                            {
                                User = user,
                                Bathroom = bathroom,
                                Title = "Hey!",
                                Message = "¡El baño " + bathroom.ID.ToString() + " está libre y es tu turno!"
                            };

                            PhotonHub.SendMessage(notification);
                        }
                        // Send an advance notification to the rest of the users in the line
                        else
                        {
                            //BathStatus bathStatus = new BathStatus()
                            //{
                            //    Title = "Hey!",
                            //    Message = "El baño " + bathroom.ID.ToString() + " está libre y sos el " + (i + 1) + "º en la fila",
                            //    BathId = bathroom.ID,
                            //    IsOccupied = bathroom.IsOccupied
                            //};
                            Notification notification = new Notification()
                            {
                                User = user,
                                Bathroom = bathroom,
                                Title = "Hey!",
                                Message = "El baño " + bathroom.ID.ToString() + " está libre y sos el " + (i + 1) + "º en la fila"
                            };
                            PhotonHub.SendMessage(notification);
                        }
                    }
                }
                // If the bath was occupied, ask the first user in line if he is in there, in order to prevent
                // another user from taking his/her place, making him/her go to the end of the line again
                else
                {
                    User user = usersInLine.First();

                    //BathStatus bathStatus = new BathStatus()
                    //{
                    //    Title = "Hey!",
                    //    Message = "El baño " + bathroom.ID.ToString() + " fue ocupado y tenías el primer lugar, ¿fuiste vos?",
                    //    BathId = bathroom.ID,
                    //    IsOccupied = bathroom.IsOccupied
                    //};
                    Notification notification = new Notification()
                    {
                        User = user,
                        Bathroom = bathroom,
                        Title = "Hey!",
                        Message = "El baño " + bathroom.ID.ToString() + " fue ocupado y tenías el primer lugar, ¿fuiste vos?"
                    };
                    PhotonHub.SendMessage(notification);
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
            BathroomLine bathLine = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>).First(a => a.Bathroom.ID == bathId);
            List<User> usersInLine = bathLine.UsersLine;

            if (usersInLine.Count > 0)
            {
                //string userId = bathLine.First();
                User user = usersInLine.First();

                if (removeFromAll)
                {
                    Unsubscribe(1, user.ID);
                    Unsubscribe(2, user.ID);
                    Unsubscribe(3, user.ID);
                }
                else
                {
                    Unsubscribe(bathId, user.ID);
                }
            }

            bathLine.LastTimesFirstChanged = DateTime.Now;
        }

        /// <summary>
        /// Method called when the first user in line indicates that he/she was not the one occupying the bath
        /// </summary>
        /// <param name="bathId"></param>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public void KeepFirstPosition(int bathId)
        {
            (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[bathId - 1] = false;
        }

    }
}
