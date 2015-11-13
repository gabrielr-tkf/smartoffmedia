using Photon.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Photon.WebAPI.Controllers
{
    public class DeviceController : ApiController
    {
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
        public DeviceVariableGetResponse GetPIRStatus()
        {
            DeviceVariableGetResponse response = new DeviceVariableGetResponse();
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

    }
}
