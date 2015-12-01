using Microsoft.AspNet.SignalR;
using Photon.Entities;
using Photon.Services;
using Photon.WebAPI.Classes;
using Photon.WebAPI.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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
                Bathroom bathroom = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>).First(a=> a.Bathroom.ID == bathId).Bathroom;
                
                if (!isOccupied)
                {
                    DateTime occupiedTime = bathroom.LastOccupiedTime;

                    Logger.LogBathUsage(bathroom);

                    // This line is repeated in the if and else sentences, because it's better to have
                    // it next to the next line, to prevent concurrency issues
                    bathroom.IsOccupied = isOccupied;
                    bathroom.LastFreedTime = DateTime.Now;

                    NotificationController notificationController = new NotificationController();
                    notificationController.Publish(bathroom);
                }
                else
                {
                    bathroom.IsOccupied = isOccupied;
                    bathroom.LastOccupiedTime = DateTime.Now;
                }                

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