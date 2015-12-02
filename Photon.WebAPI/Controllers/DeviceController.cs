using Photon.Entities;
using Photon.WebAPI.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Photon.WebAPI.Controllers
{
    public class DeviceController : ApiController
    {

        [System.Web.Http.AcceptVerbs("GET")]
        public void LogSensorActivity(string deviceId, string sensorType, string sensorValue)
        {

            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);
            Bathroom bathroom = bathroomLines.First(a => a.Bathroom.PhotonDevice.ID == deviceId).Bathroom;

            Device device = bathroom.PhotonDevice;

            bool isOccupied = bathroom.IsOccupied;

            //PirSensor
            if (sensorType == "1")
            {
                if (sensorValue == "false")
                {
                    device.consecutive0s++;
                    device.consecutive1s = 0;
                }
                else if (sensorValue == "true")
                {
                    device.consecutive0s = 0;
                    device.consecutive1s++;
                }

                int reportsRequiredToOccupy = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.ReportsRequiredToOccupy]);
                int reportsRequiredToFree = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.ReportsRequiredToFree]);

                if (!isOccupied)
                {
                    // Checking that the last 2 inputs were '1'
                    if (device.consecutive1s >= reportsRequiredToOccupy)
                    {
                        LogController lC = new LogController();
                        lC.LogStateChange(bathroom.ID, true);
                    }
                }
                else
                {
                    // Checking that the last 4 inputs were '0'
                    if (device.consecutive0s >= reportsRequiredToFree)
                    {
                        LogController lC = new LogController();
                        lC.LogStateChange(bathroom.ID, false);
                    }
                }
            }
            else {

                device.PhotoSensorValue = sensorValue;
                //To Test other sensors
            }
        }




        [System.Web.Http.AcceptVerbs("GET")]
        public DeviceGetResponse Get()
        {
            DeviceGetResponse response = new DeviceGetResponse();
            try
            {
                response.Device = Photon.Services.PhotonController.GetDeviceStatus();
                //Everything OK
                response.Status = "200";
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error";
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET")]
        public DeviceGetVariableResponse GetPIRStatus()
        {
            DeviceGetVariableResponse response = new DeviceGetVariableResponse();
            try
            {
                response.Variable = Photon.Services.PhotonController.GetDevicePIRStatus();
                //Everything OK
                response.Status = "200";
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error";
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public DeviceGetResponse GetOAuthToken()
        {

            DeviceGetResponse response = new DeviceGetResponse();
            try
            {
                //response.Device = Photon.Services.PhotonController.GetDeviceStatus();


                Photon.Services.PhotonController.GetToken();

                //Everything OK
                response.Status = "200";
                response.Message = "Success";
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
