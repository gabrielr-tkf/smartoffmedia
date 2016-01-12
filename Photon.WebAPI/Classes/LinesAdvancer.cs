using Photon.Entities;
using Photon.WebAPI.Controllers;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Photon.WebAPI.Classes
{
    public class LinesAdvancer
    {
        /// <summary>
        /// Looks for baths that are free and advances the line in case a bath has been free for more than
        /// 1 minute (the first person is removed from the line)
        /// </summary>
        public static void UnusedBathsVerifier()
        {
            NotificationController notificationController = new NotificationController();

            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);
            DateTime lastFreedTime;
            TimeSpan span;
            int ms;
            Bathroom bathroom;

            while (true)
            {

                for (int i = 0; i < bathroomLines.Count; i++)
                {
                    BathroomLine bathroomLine = bathroomLines[i];
                    bathroom = bathroomLine.Bathroom;
                    lastFreedTime = bathroom.LastFreedTime;
                    span = DateTime.Now - lastFreedTime;
                    ms = (int)span.TotalMilliseconds;

                    // After 60 seconds since the last bath exit time, if the bath is still free,
                    // advances the line and sends a notification to all the users waiting for that
                    // bath. The last freed time is set as DateTime.Now (resetting the seconds count to 0)


                    if (ms > 60000 && !(bathroom.IsOccupied))
                    {
                        if (bathroomLine.UsersLine.Count != 0)
                        {
                            bathroom.LastFreedTime = DateTime.Now;
                        }
                        
                        notificationController.AdvanceLine(bathroom.ID, false);
                        notificationController.Publish(bathroom);

                    }

                }

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Method used to confirm that the first user in line, is effectively occupying the bath
        /// </summary>
        public static void FirstInLineOccupancyVerifier()
        {
            NotificationController notificationController = new NotificationController();

            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);
            Bathroom bathroom;
            DateTime lastOccupiedTime;
            DateTime lastTimeFirstChanged;
            TimeSpan span;
            int ms;
            while (true)
            {

                for (int i = 0; i < bathroomLines.Count; i++)
                {
                    bathroom = bathroomLines[i].Bathroom;
                    lastOccupiedTime = bathroom.LastOccupiedTime;
                    lastTimeFirstChanged = bathroomLines[i].LastTimesFirstChanged;
                    span = DateTime.Now - lastOccupiedTime;
                    ms = (int)span.TotalMilliseconds;

                    // After 45 seconds since the last occupancy time, if the user didn't indicate that
                    // he/she was not the one who occupied the bath, it's assumed that he/she was the one,
                    // so we remove him/her from all the lines
                    if (ms > 45000 && (CacheManager.Get(Constants.OccupiedByFirstInLine) as List<bool>)[i])
                    {
                        // If the line had already been advanced, don't do it again. We check this by
                        // comparing the last time the bath was occupied and the last time we advanced
                        // the line
                        if (lastOccupiedTime > lastTimeFirstChanged)
                        {
                            notificationController.AdvanceLine(i + 1, true);
                        }
                    }
                }

                Thread.Sleep(3000);
            }
        }
    }
}