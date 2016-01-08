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
        public static Device GetDeviceStatus(string deviceId)
        {
            try
            {
                PhotonRESTService service = new PhotonRESTService();

                //Dictionary<string, string> additionalParameters = new Dictionary<string, string>();
                //additionalParameters.Add("area_id", "1"); //World
                //additionalParameters.Add("authorized", "yes"); //authorized

                string json = service.GetJSON(deviceId);

                //dynamic context = JObject.Parse(json);
                Device device = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(json);
                return device;
            }
            catch (Exception ex)
            {
                return new Device(){ Connected = false};
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
