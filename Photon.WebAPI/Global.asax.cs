﻿using Photon.Entities;
using Photon.WebAPI.Classes;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            //Chrome Plugin IDs lists - Used for SIGNALR
            List<User> UsersList = new List<User>();
            CacheManager.Add(Constants.UsersList, UsersList);  


            //All available bathrooms
            Bathroom wcMen = new Bathroom()
            {
                ID = 1,
                PhotonDevice = new Device(){ ID = "330034000d47343432313031", ProximityValue = 100 },
                IsOccupied = false,
                Name = "WCMen",
                LastFreedTime = DateTime.Now,
                LastOccupiedTime = DateTime.Now
            };
            Bathroom wcWoman = new Bathroom()
            {
                ID = 2,
                PhotonDevice = new Device() { ID = "UnknownBath2", ProximityValue = 100 },
                IsOccupied = false,
                Name = "wcWoman",
                LastFreedTime = DateTime.Now,
                LastOccupiedTime = DateTime.Now
            };
            Bathroom wcMix = new Bathroom()
            {
                ID = 3,
                PhotonDevice = new Device() { ID = "UnknownBath3", ProximityValue = 100 },
                IsOccupied = false,
                Name = "wcMix",
                LastFreedTime = DateTime.Now,
                LastOccupiedTime = DateTime.Now
            };

            // List of waiting Line (Users)
            BathroomLine linebth1 = new BathroomLine()
            {
                Bathroom = wcMen,
                LastTimesFirstChanged = DateTime.Now
            };          

            BathroomLine linebth2 = new BathroomLine()
            {
                Bathroom = wcWoman,
                LastTimesFirstChanged = DateTime.Now
            };

            BathroomLine linebth3 = new BathroomLine()
            {
                Bathroom = wcMix,
                LastTimesFirstChanged = DateTime.Now
            };

            List<BathroomLine> bathlines = new List<BathroomLine>();
            bathlines.Add(linebth1);
            bathlines.Add(linebth2);
            bathlines.Add(linebth3);

            CacheManager.Add(Constants.BathLines, bathlines);  

            // Flags that the first user in line can set false when somebody else occupied the bathroom instead of him/her
            CacheManager.Add(Constants.OccupiedByFirstInLine, new List<bool>(new bool[] { true, true, true}));

            new Thread(() =>
            {
                LinesAdvancer.UnusedBathsVerifier();
            }).Start();

            new Thread(() =>
            {
                LinesAdvancer.FirstInLineOccupancyVerifier();
            }).Start();

            new Thread(() =>
            {
                DeviceProcessor.ProcessDevicesState("330034000d47343432313031");
            }).Start();

            // TODO: This line is only for debugging purposes. It has to be removed
            CacheManager.Add("Notifications", new List<string>());
            
        }
    }
}
