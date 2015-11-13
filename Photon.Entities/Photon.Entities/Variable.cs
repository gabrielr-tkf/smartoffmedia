using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    //  {
    //      "cmd": "VarReturn",
    //      "name": "PIRState",
    //      "result": 0,
    //      "coreInfo": {
    //          "last_app": "",
    //          "last_heard": "2015-11-12T16:20:20.229Z",
    //          "connected": true,
    //          "last_handshake_at": "2015-11-12T16:19:32.732Z",
    //          "deviceID": "330034000d47343432313031",
    //          "product_id": 6
    //      }
    //  }
    public class Variable
    {
        public string CMD { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }

        // TODO: 
            //    "coreInfo": {
            //          "last_app": "",
            //          "last_heard": "2015-11-12T16:20:20.229Z",
            //          "connected": true,
            //          "last_handshake_at": "2015-11-12T16:19:32.732Z",
            //          "deviceID": "330034000d47343432313031",
            //          "product_id": 6
            //      }
    }
}
