using Newtonsoft.Json.Linq;
using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Services
{
    public class PhotonController
    {
        //This will be used to lock the "cacheUbication" object. To stop multiple calls to the database,
        //when the cache object is either expired or about to be expired.
        private static object syncRoot = new Object();


        public static Device GetDeviceStatus(string deviceId)
        {
            string cacheLocation = "DeviceStatus." + deviceId;
            try
            {
                if (Photon.Services.Utilities.CacheManager.ValidatExistence(cacheLocation))
                {
                    return Photon.Services.Utilities.CacheManager.Get(cacheLocation) as Device;
                }
                else
                {
                    lock (syncRoot)
                    {
                        if (!Photon.Services.Utilities.CacheManager.ValidatExistence(cacheLocation))
                        {
                            PhotonRESTService service = new PhotonRESTService();

                            //Dictionary<string, string> additionalParameters = new Dictionary<string, string>();
                            //additionalParameters.Add("area_id", "1"); //World
                            //additionalParameters.Add("authorized", "yes"); //authorized

                            string json = service.GetJSON(deviceId);

                            Device device = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(json);
                            Photon.Services.Utilities.CacheManager.Add(cacheLocation, deviceId, 1);
                            return device;
                        }
                        else
                        {
                            return Photon.Services.Utilities.CacheManager.Get(cacheLocation) as Device;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Device() { Connected = false };
            }


        }
        //Get movement YES/NO
        public static Variable GetDevicePIRStatus()
        {
            PhotonRESTService service = new PhotonRESTService();

            string json = service.GetJSONVariable("PIRState");

            //dynamic context = JObject.Parse(json);
            Variable variable = Newtonsoft.Json.JsonConvert.DeserializeObject<Variable>(json);
            return variable;

        }
        public static string GetToken()
        {
            PhotonRESTService service = new PhotonRESTService();


            service.GetJSONOAuth();

            return "";

        }
    }
}
