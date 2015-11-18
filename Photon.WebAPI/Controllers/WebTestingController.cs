using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photon.WebAPI.Controllers
{
    public class WebTestingController : Controller
    {
        [System.Web.Http.AcceptVerbs("GET")]
        public ActionResult ShowStateChange()
        {
            //LogController lC = new LogController();
            //lC.LogStateChange(bathId, isOccupied);

            return View();
        }
    }
}