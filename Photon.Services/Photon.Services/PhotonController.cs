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
        public static Device GetDeviceStatus()
        {
            PhotonRESTService service = new PhotonRESTService();

            //Dictionary<string, string> additionalParameters = new Dictionary<string, string>();
            //additionalParameters.Add("area_id", "1"); //World
            //additionalParameters.Add("authorized", "yes"); //authorized

            string json = service.GetJSON();

            //dynamic context = JObject.Parse(json);
            Device device = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(json);
            return device;

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
