using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Photon.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpContext.Current.Cache[Constants.OccupiedBaths] = new List<bool>(new bool[]{false, false, false});
            HttpContext.Current.Cache[Constants.LastOccupiedTimes] = new List<DateTime>(new DateTime[] { DateTime.Now, DateTime.Now, DateTime.Now }); ;            
            HttpContext.Current.Cache[Constants.BathQueues] = new List<Queue<string>>(new Queue<string>[] {new Queue<string>(), new Queue<string>(), new Queue<string>()});
        }
    }
}
