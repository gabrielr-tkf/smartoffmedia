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
            BathGetStatusResponse response = new BathGetStatusResponse();
            response.Message = "Success";
            response.Status = "200";
            response.BathStatusList = CacheManager.Get(Constants.BathLines) as List<BathroomLine>;

            return response;
        }
    }
}
