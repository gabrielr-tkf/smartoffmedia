using Microsoft.AspNet.SignalR;
using Photon.WebAPI.Classes;
using Photon.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Photon.WebAPI.Controllers
{
    public class LogController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET")]
        public LogStateChangeResponse LogStateChange(int bathId, bool isOccupied)
        {
            LogStateChangeResponse response = new LogStateChangeResponse();
           
            try
            {
                // Store data in DB
                if (HttpContext.Current.Cache["LogsTable"] == null)
                {
                    HttpContext.Current.Cache["LogsTable"] = new List<Tuple<int, bool, DateTime>>();
                    ((List<Tuple<int, bool, DateTime>>)HttpContext.Current.Cache["LogsTable"]).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
                }
                else
                {
                    ((List<Tuple<int, bool, DateTime>>)HttpContext.Current.Cache["LogsTable"]).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
                }



                NotificationController notificationController = new NotificationController();
                notificationController.Publish(bathId.ToString(), isOccupied);



                response.Status = "200";
                response.Message = "State successfully logged";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error";
            }

           return response;
        }
    }
}