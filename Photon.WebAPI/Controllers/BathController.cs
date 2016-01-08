using Photon.Entities;
using Photon.WebAPI.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Photon.WebAPI.Controllers
{
    public class BathController : ApiController
    {
      
        
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.AcceptVerbs("GET")]
        public BathGetStatusResponse GetStatus()
        {
            List<BathroomLine> bathLines =  CacheManager.Get(Constants.BathLines) as List<BathroomLine>;

            foreach (var bathLine in bathLines)
            {
                //Update "connected" status of device
                string deviceId = bathLine.Bathroom.PhotonDevice.ID;
                bathLine.Bathroom.PhotonDevice.Connected = Photon.Services.PhotonController.GetDeviceStatus(deviceId).Connected;
            }

            BathGetStatusResponse response = new BathGetStatusResponse();
            response.Message = "Success";
            response.Status = "200";
            response.BathStatusList = bathLines;

           

            return response;
        }
    }
}
