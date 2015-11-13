﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    //  {
    //      "id": "330034000d47343432313031",
    //      "name": "Photon Takeoff",
    //      "connected": true,
    //      "variables": {
    //          "PIRState": "int32"
    //      },
    //      "functions": [],
    //      "cc3000_patch_version": "wl0: Nov  7 2014 16:03:45 version 5.90.230.12 FWID 01-7b5c1811",
    //      "product_id": 6,
    //      "last_heard": "2015-11-12T15:36:20.426Z"
    //  }
    public class Device
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Connected { get; set; }
        //public string Variables { get; set; }
        public string[] Functions { get; set; }
        public string Product_ID { get; set; }
        public string Last_Heard { get; set; }

    }
}
