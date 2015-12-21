using Photon.Entities;
using Photon.WebAPI.Controllers;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;

namespace Photon.WebAPI.Classes
{
    public class DeviceProcessor
    {
        /// <summary>
        /// Procceses the devices state depending on the bathroom
        /// </summary>
        /// <param name="deviceId">The ID of the Photon device implanted on the bathroom</param>
        public static void ProcessDevicesState(string deviceId)
        {
            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);
            Bathroom bathroom = bathroomLines.First(a => a.Bathroom.PhotonDevice.ID == deviceId).Bathroom;

            if (bathroom.ID == 1)
            {
                ProcessDeviceMethod1(bathroom);
            }
            else if (bathroom.ID == 2 || bathroom.ID == 3)
            {
                ProcessDeviceMethod2(bathroom);
            }
            
        }

        /// <summary>
        /// Uses a PIR motion sensor, a Proximity sensor and a Photovoltaic sensor
        /// </summary>
        /// <param name="bathroom">The bathroom where the device is implanted</param>
        private static void ProcessDeviceMethod1(Bathroom bathroom)
        {
            Device device = bathroom.PhotonDevice;
            // Milliseconds elapsed since the last time the PIR sensor reported status change
            TimeSpan PIRSpan;
            int PIRMs;
            // Milliseconds elapsed since the last time the Photo sensor reported status change
            TimeSpan PhotoSpan;
            int PhotoMs;
            // Milliseconds elapsed since the last time the Proximity sensor reported status change
            TimeSpan ProximitySpan;
            int ProximityMs;

            int PIRSecondsRequiredToOccupy = int.Parse(ConfigurationManager.AppSettings[Constants.PIRSecondsRequiredToOccupy]);
            int PIRSecondsRequiredToFree = int.Parse(ConfigurationManager.AppSettings[Constants.PIRSecondsRequiredToFree]);
            int lightOnThreshold = int.Parse(ConfigurationManager.AppSettings[Constants.LightOnThreshold]);
            int proximityThreshold = int.Parse(ConfigurationManager.AppSettings[Constants.ProximityThreshold]);
            LogController lC = new LogController();

            while (true)
            {
                PIRSpan = DateTime.Now - device.LastPIRReportTime;
                PIRMs = (int)PIRSpan.TotalMilliseconds;

                PhotoSpan = DateTime.Now - device.LastPhotoReportTime;
                PhotoMs = (int)PhotoSpan.TotalMilliseconds;

                ProximitySpan = DateTime.Now - device.LastProximityReportTime;
                ProximityMs = (int)ProximitySpan.TotalMilliseconds;

                // If there is a person close the proximity sensor, the bath is occupied
                if (device.ProximityValue < proximityThreshold)
                {
                    if (!bathroom.IsOccupied && ProximityMs > 3000)
                    {
                        lC.LogStateChange(bathroom.ID, true);
                    }
                }
                // If the Proximity Sensor was not helpful to determine the bath status
                else
                {
                    // Avoid changing the state when the user is moving from the WC area to the basin area
                    if (ProximityMs > 5000)
                    {
                        // If there is movement and the light was turned on a few seconds ago, the bath is occupied
                        if (device.PIRSensorValue == 1 && device.PhotoSensorValue >= lightOnThreshold && PhotoMs < 3000)
                        {
                            if (!bathroom.IsOccupied)
                            {
                                lC.LogStateChange(bathroom.ID, true);
                            }
                        }
                        // If there is not movement and the light is off, the bath is free
                        else if (device.PIRSensorValue == 0 && device.PhotoSensorValue < lightOnThreshold)
                        {
                            if (bathroom.IsOccupied)
                            {
                                lC.LogStateChange(bathroom.ID, false);
                            }
                        }
                        // If the PhotoSensor was not helpful to determine the bath status
                        else
                        {
                            // If the device is reporting '1' and the bathroom is free
                            if (device.PIRSensorValue == 1 && !bathroom.IsOccupied)
                            {
                                // Avoid logging the state change more than once in a row
                                if (device.LastPIRReportTime > bathroom.LastOccupiedTime)
                                {
                                    // If some seconds have passed with uninterrupted 'true' state
                                    if (PIRMs > PIRSecondsRequiredToOccupy * 1000)
                                    {
                                        lC.LogStateChange(bathroom.ID, true);
                                    }
                                }
                            }
                            // If the device is reporting '0' and the bathroom is occupied
                            else if (device.PIRSensorValue == 0 && bathroom.IsOccupied)
                            {
                                // Avoid logging the state change more than once in a row
                                if (device.LastPIRReportTime > bathroom.LastFreedTime)
                                {
                                    // If some seconds have passed with a uninterrupted 'false' state
                                    if (PIRMs > PIRSecondsRequiredToFree * 1000)
                                    {
                                        lC.LogStateChange(bathroom.ID, false);
                                    }
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Uses a PIR motion sensor and a Proximity sensor
        /// </summary>
        /// <param name="bathroom">The bathroom where the device is implanted</param>
        private static void ProcessDeviceMethod2(Bathroom bathroom)
        {
            Device device = bathroom.PhotonDevice;
            // Milliseconds elapsed since the last time the PIR sensor reported status change
            TimeSpan PIRSpan;
            int PIRMs;
            // Milliseconds elapsed since the last time the Proximity sensor reported status change
            TimeSpan ProximitySpan;
            int ProximityMs;

            int PIRSecondsRequiredToOccupy = int.Parse(ConfigurationManager.AppSettings[Constants.PIRSecondsRequiredToOccupy]);
            int PIRSecondsRequiredToFree = int.Parse(ConfigurationManager.AppSettings[Constants.PIRSecondsRequiredToFree]);
            int proximityThreshold = int.Parse(ConfigurationManager.AppSettings[Constants.ProximityThreshold]);
            LogController lC = new LogController();

            while (true)
            {
                PIRSpan = DateTime.Now - device.LastPIRReportTime;
                PIRMs = (int)PIRSpan.TotalMilliseconds;

                ProximitySpan = DateTime.Now - device.LastProximityReportTime;
                ProximityMs = (int)ProximitySpan.TotalMilliseconds;

                // If there is a person close the proximity sensor, the bath is occupied
                if (device.ProximityValue < proximityThreshold)
                {
                    if (!bathroom.IsOccupied && ProximityMs > 3000)
                    {
                        lC.LogStateChange(bathroom.ID, true);
                    }
                }
                // If the Proximity Sensor was not helpful to determine the bath status
                else
                {
                    // Avoid changing the state when the user is moving from the WC area to the basin area
                    if (ProximityMs > 5000)
                    {
                        // If the device is reporting '1' and the bathroom is free
                        if (device.PIRSensorValue == 1 && !bathroom.IsOccupied)
                        {
                            // Avoid logging the state change more than once in a row
                            if (device.LastPIRReportTime > bathroom.LastOccupiedTime)
                            {
                                // If some seconds have passed with uninterrupted 'true' state
                                if (PIRMs > PIRSecondsRequiredToOccupy * 1000)
                                {
                                    lC.LogStateChange(bathroom.ID, true);
                                }
                            }
                        }
                        // If the device is reporting '0' and the bathroom is occupied
                        else if (device.PIRSensorValue == 0 && bathroom.IsOccupied)
                        {
                            // Avoid logging the state change more than once in a row
                            if (device.LastPIRReportTime > bathroom.LastFreedTime)
                            {
                                // If some seconds have passed with a uninterrupted 'false' state
                                if (PIRMs > PIRSecondsRequiredToFree * 1000)
                                {
                                    lC.LogStateChange(bathroom.ID, false);
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}