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
                device.PIRSensorValue = int.Parse(sensorValue);
                device.LastPIRReportTime = DateTime.Now;
            }
            //ProximitySensor
            else if (sensorType == "2")
            {

                device.ProximityValue = int.Parse(sensorValue);
                device.LastProximityReportTime = DateTime.Now;
            }
            //PhotoResistor
            else if(sensorType == "3"){

                device.PhotoSensorValue = int.Parse(sensorValue);
                device.LastPhotoReportTime = DateTime.Now;
            }

            //Services.Logger.LogDeviceReport(deviceId, sensorType, sensorValue);
        }

        //Get the device status by bathId
        [System.Web.Http.AcceptVerbs("GET")]
        public DeviceGetResponse GetByBathId(int bathId)
        {
            DeviceGetResponse response = new DeviceGetResponse();

            if (CacheManager.ValidatExistence(Constants.BathLines))
            {
                //Find photon id used in the bathroom
                List<BathroomLine> bathLines = CacheManager.Get(Constants.BathLines) as List<BathroomLine>;
                string deviceId = bathLines.Where(a => a.Bathroom.ID == bathId).First().Bathroom.PhotonDevice.ID;

                try
                {
                    response.Device = Photon.Services.PhotonController.GetDeviceStatus(deviceId);
                    //Everything OK
                    response.Status = "200";
                    response.Message = "Success";
                }
                catch (Exception ex)
                {
                    response.Status = "500";
                    response.Message = "Internal Server Error";
                }
            }
            else
            {
                response.Status = "204";
                response.Message = "No content";
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public DeviceGetResponse Get(string deviceId)
        {
            DeviceGetResponse response = new DeviceGetResponse();
            try
            {
                response.Device = Photon.Services.PhotonController.GetDeviceStatus(deviceId);
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
