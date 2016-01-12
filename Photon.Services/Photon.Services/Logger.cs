using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Photon.Services
{
    public class Logger
    {
        public static void LogBathUsage(Bathroom bathroom)
        {
            new Thread(() =>
            {
                Photon.DataAccess.Logger.LogBathUsage(bathroom);
            }).Start();            
        }

        public static void LogDeviceReport(string deviceId, string sensorId, string value)
        {
            new Thread(() =>
            {
                Photon.DataAccess.Logger.LogDeviceReport(deviceId, sensorId, value);
            }).Start();
        }
        public static void LogError(string controller, string method, Exception exception, string clientIp, string url)
        {
        
    
            //new Thread(() =>
            //{
                Photon.DataAccess.Logger.LogError(controller, method, exception,clientIp, url);
            //}).Start();
        }
    }
}
