using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    //  {
    //      "token_type": "bearer",
    //      "access_token": "80830377ba74758be595c1636fea0718b06cc56a",
    //      "expires_in": 7776000,
    //      "refresh_token": "3e024353e3a53ca966f6ccd45da626babd42a66d"
    //  }
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }  
    }
}
