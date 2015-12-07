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

            //PirSensor
            if (sensorType == "1")
            {
                device.PIRSensorValue = sensorValue;
                device.LastPIRReportTime = DateTime.Now;
            }
            else if(sensorType == "3"){

                device.PhotoSensorValue = int.Parse(sensorValue);
                device.LastPhotoReportTime = DateTime.Now;
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
