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
    public class DeviceProcessor
    {

        public static void ProcessDeviceState(string deviceId)
        {
            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);
            Bathroom bathroom = bathroomLines.First(a => a.Bathroom.PhotonDevice.ID == deviceId).Bathroom;
            Device device = bathroom.PhotonDevice;
            TimeSpan span;
            int ms;

            while (true)
            {
                span = DateTime.Now - device.LastPIRReportTime;
                ms = (int)span.TotalMilliseconds;

                // If the device is reporting '1' and the bathroom is occupied
                if (device.PIRSensorValue == "1" && !bathroom.IsOccupied)
                {
                    // Avoid logging the state change more than once in a row
                    if (device.LastPIRReportTime > bathroom.LastOccupiedTime)
                    {
                        // If 8 seconds have passed with uninterrupted 'true' state
                        if (ms > 8000)
                        {
                            LogController lC = new LogController();
                            lC.LogStateChange(bathroom.ID, true);
                        }
                    }
                }
                // If the device is reporting '0' and the bathroom is free
                else if (device.PIRSensorValue == "0" && bathroom.IsOccupied)
                {
                    // Avoid logging the state change more than once in a row
                    if (device.LastPIRReportTime > bathroom.LastFreedTime)
                    {
                        // If 16 seconds have passed with a uninterrupted 'false' state
                        if (ms > 16000)
                        {
                            LogController lC = new LogController();
                            lC.LogStateChange(bathroom.ID, false);
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}