using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for RESTService
/// </summary>
public class WebRESTService
{

    private struct BasicConfiguration
    {

        public static string BaseURL
        {
            get
            {
                return ConfigurationManager.AppSettings["APIBaseURL"].ToString();
            }
        }


    }
    public List<Photon.Entities.Bathroom> GetBathStatus()
    {
        string kind = "bath";
        string method = "GetStatus";

        string uri = BasicConfiguration.BaseURL + "/" + kind + "/" + method;

        string json = GetCall(uri);

        JObject jsonObject = JObject.Parse(json);
        var tokens = jsonObject["BathStatusList"].Children();

        List<Photon.Entities.Bathroom> bathrooms = new List<Photon.Entities.Bathroom>();

        string jsonExample = "{   'PhotonDevice': {" +
            "'ID': '330034000d47343432313031'," + 
            "'Name': null,"+
            "'Connected': true,"+
            "'Functions': null,"+
            "'Product_ID': null,"+
            "'LastPIRReportTime': '2016-04-01T16:14:19.108877-03:00',"+
            "'LastPhotoReportTime': '2016-04-01T16:14:17.4838499-03:00',"+
            "'LastProximityReportTime': '2016-04-01T11:53:39.50055-03:00',"+
            "'PIRSensorValue': 0,"+
            "'PhotoSensorValue': 49,"+
            "'ProximityValue': 3548"+
          "},"+
          "'ID': 1,"+
          "'Name': 'WCMen',"+
          "'IsOccupied': false,"+
          "'LastOccupiedTime': '2016-04-01T16:14:14.0463886-03:00',"+
          "'LastFreedTime': '2016-04-01T16:14:20.0932112-03:00',"+
          "'FreeFor': '00:01:46.2140747'" +
        "}";

        foreach (JToken item in tokens)
        {
          string temp =   item.Children().ToString();
            Photon.Entities.Bathroom bath = Newtonsoft.Json.JsonConvert.DeserializeObject<Photon.Entities.Bathroom>(jsonExample);
          bathrooms.Add(bath);
        }

        return bathrooms;
    }

    private string GetCall(string uri)
    {
        Stream stream = null;
        StreamReader streamReader;
        try
        {
            //uri = "https://www.google.com";
            //Creates a new WebRequest object with the URI and specifies its method.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.Method = "POST";

            //Obtains the response from the website.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Opens a stream from the website then pipes it to a StreamReader with proper encoding.
            stream = response.GetResponseStream();
            streamReader = new StreamReader(stream, Encoding.UTF8);

            string jsonResult = streamReader.ReadToEnd();

            return jsonResult;
        }
        //If an error occured, return null
        catch (WebException e)
        {
            return null;
        }
    }

}