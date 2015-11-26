using Microsoft.AspNet.SignalR;
using Photon.DataAccess;
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
                (CacheManager.Get(Constants.OccupiedBaths) as List<bool>)[bathId - 1] = isOccupied;

                if (!isOccupied)
                {
                    DateTime occupiedTime = (CacheManager.Get(Constants.LastOccupiedTimes) as List<DateTime>)[bathId - 1];

                    Logger.LogBathUsage(bathId, occupiedTime, DateTime.Now);

                    NotificationController notificationController = new NotificationController();
                    notificationController.Publish(bathId, isOccupied);

                    (CacheManager.Get(Constants.LastFreedTimes) as List<DateTime>)[bathId] = DateTime.Now;
                }
                else
                {
                    (CacheManager.Get(Constants.LastOccupiedTimes) as List<DateTime>)[bathId] = DateTime.Now;
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

        /// <summary>
        /// Return the state of 
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public LogGetStateResponse GetState()
        {

            LogGetStateResponse response = new LogGetStateResponse()
            {
                BathStatus = new List<Photon.Entities.BathStatus>(),
                Message = "Success",
                Status = "200"
            };

            int id = 1;
            foreach (bool bathState in (CacheManager.Get(Constants.OccupiedBaths) as List<bool>))
            {
               
                response.BathStatus.Add(new Photon.Entities.BathStatus()
                {
                    BathId = id,
                    IsOccupied = bathState
                });
                id++;
            }

            return response;
        }
    }
}