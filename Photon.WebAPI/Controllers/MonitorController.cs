using Photon.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photon.WebAPI.Controllers
{
    public class MonitorController : Controller
    {
        public ActionResult Index()
        {
            List<BathroomLine> bathroomLines = (CacheManager.Get(Constants.BathLines) as List<BathroomLine>);

            ViewBag.Line1 = "";
            ViewBag.Line2 = "";
            ViewBag.Line3 = "";

            for (int i = 0; i < bathroomLines.Count; i++)
            {
                foreach (User u in bathroomLines[i].UsersLine)
                {
                    if(i == 0) ViewBag.Line1 += u.ID.Split('-')[0] + " - ";
                    if (i == 1) ViewBag.Line2 += u.ID.Split('-')[0] + " - ";
                    if (i == 2) ViewBag.Line3 += u.ID.Split('-')[0] + " - ";
                }
            }

            return View();
        }
    }
}