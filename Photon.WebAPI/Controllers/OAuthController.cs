using Photon.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Photon.WebAPI.Controllers
{
    public class OAuthController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public DeviceGetResponse Get()
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

