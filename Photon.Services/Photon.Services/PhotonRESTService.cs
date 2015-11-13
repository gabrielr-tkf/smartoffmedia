using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Services
{
    internal class PhotonRESTService
    {
        private struct BasicConfiguration
        {
            public static string AuthorizationToken
            {
                get
                {
                    return ConfigurationManager.AppSettings["PhotonAPIAccessToken"].ToString();
                }
            }
            public static string BaseURL
            {
                get
                {
                    return ConfigurationManager.AppSettings["PhotonAPIBaseURL"].ToString();
                }
            }
              public static string BaseOauthURL
            {
                get
                {
                    return ConfigurationManager.AppSettings["PhotonAPIOauthBaseURL"].ToString();
                }
            }
          
        }
        public string GetJSON()
        {

            string deviceID = "330034000d47343432313031";
            string kind = "devices";

            string uri = BasicConfiguration.BaseURL + "/" + kind + "/" + deviceID + "/"
                 + "?" +
                "access_token=" + BasicConfiguration.AuthorizationToken;


            return GetCall(uri);
        }
        public string GetJSONVariable(string variableName)
        {
            string deviceID = "330034000d47343432313031";
            string kind = "devices";

            string uri = BasicConfiguration.BaseURL + "/" + kind + "/" + deviceID + "/" + variableName + "/"
                 + "?" +
                "access_token=" + BasicConfiguration.AuthorizationToken;

            return GetCall(uri);
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
        //TODO:
        #region OAuth Example
        public Token GetJSONOAuth()
        {
            string baseAddress = "https://api.particle.io/oauth/";

           
                var client = new HttpClient();
              

                var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes("particle:particle"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

                var form = new Dictionary<string, string>  
               {  
                   {"grant_type", "password"},  
                   {"username", "andresg@takeoffmedia.com"},  
                   {"password", "luna32mar"},  
                   {"expires_in", "0"}
               };

                var tokenResponse = client.PostAsync(baseAddress + "token", new FormUrlEncodedContent(form)).Result;
                var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;

                return token;

        }
        private string GetCallOAuth(string uri)
        {
            Stream stream = null;
            StreamReader streamReader;
            try
            {
                //uri = "https://www.google.com";
                //Creates a new WebRequest object with the URI and specifies its method.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

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
        #endregion
    }
}
