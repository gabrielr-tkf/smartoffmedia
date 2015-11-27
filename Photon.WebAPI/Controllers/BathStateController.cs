using Photon.WebAPI.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Photon.WebAPI.Controllers
{
    public class BathStateController : ApiController
    {
        /// <summary>
        /// Looks for baths that are free and advances the line in case a bath has been free for more than
        /// 1 minute (the first person is removed from the line)
        /// </summary>
        public void UnusedBathsVerifier()
        {
            NotificationController notificationController = new NotificationController();

            while (true)
            {
                for (int i = 0; i < (CacheManager.Get(Constants.BathLines) as List<List<string>>).Count; i++)
                {
                    DateTime lastFreedTime = (CacheManager.Get(Constants.LastFreedTimes) as List<DateTime>)[i];
                    TimeSpan span = DateTime.Now - lastFreedTime;
                    int ms = (int)span.TotalMilliseconds;

                    // After 60 seconds since the last bath exit time, if the bath is still free,
                    // advances the line and sends a notification to all the users waiting for that
                    // bath. The last freed time is set as DateTime.Now (resetting the seconds count to 0)
                    if (ms > 60000 && !(CacheManager.Get(Constants.OccupiedBaths) as List<bool>)[i])
                    {
                        notificationController.AdvanceLine(i + 1, false);
                        notificationController.Publish(i + 1, false);

                        (CacheManager.Get(Constants.LastFreedTimes) as List<DateTime>)[i] = DateTime.Now;
                    }

                }

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Method used to confirm that the first user in line, is effectively occupying the bath
        /// </summary>
        public void FirstInLineOccupancyVerifier(int bathId)
        {
            NotificationController notificationController = new NotificationController();

            while (true)
            {
                for (int i = 0; i < (CacheManager.Get(Constants.BathLines) as List<List<string>>).Count; i++)
                {
                    DateTime lastOccupiedTime = (CacheManager.Get(Constants.LastOccupiedTimes) as List<DateTime>)[i];
                    DateTime lastLineAdvanceTime = (CacheManager.Get(Constants.LastLineAdvanceTimes) as List<DateTime>)[i];
                    TimeSpan span = DateTime.Now - lastOccupiedTime;
                    int ms = (int)span.TotalMilliseconds;
                    
                    // After 45 seconds since the last occupancy time, if the user didn't indicate that
                    // he/she was not the one who occupied the bath, it's assumed that he/she was the one,
                    // so we remove him/her from all the lines
                    if (ms > 45000 && (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[i])
                    {
                        // If the line had already been advanced, don't do it again. We check this by
                        // comparing the last time the bath was occupied and the last time we advanced
                        // the line
                        if (lastOccupiedTime > lastLineAdvanceTime)
                        {
                            notificationController.AdvanceLine(i + 1, true);
                        }
                    }
                }

                Thread.Sleep(3000);
            }
        }

          [System.Web.Http.AcceptVerbs("GET")]
        public BathStateResponse GetAllBathStatus()
        {
           List<bool> bathOccupancy = CacheManager.Get(Constants.OccupiedBaths) as List<bool>;

           BathStateResponse response = new BathStateResponse();
           response.Message = "Success";
           response.Status = "200";
           response.BathStatusList = new List<Photon.Entities.BathStatus>();

           int bathid = 1;
           foreach (var item in bathOccupancy)
           {
               Photon.Entities.BathStatus bathStatus = new Photon.Entities.BathStatus()
               {
                   BathId = bathid,
                   IsOccupied = item
               };
               response.BathStatusList.Add(bathStatus);
               bathid++;

           }

           return response;

           
        }
    }
}