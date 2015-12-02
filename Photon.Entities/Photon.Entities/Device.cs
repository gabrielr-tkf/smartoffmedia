using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    // GetDeviceStatus
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
        public bool Connected { get; set; }
        //public string Variables { get; set; }
        public string[] Functions { get; set; }
        public string Product_ID { get; set; }
        public DateTime LastPIRReportTime { get; set; }
        // Attributes used to store the last reports from the device

        public string PIRSensorValue { get; set; }
        public string PhotoSensorValue { get; set; }

    }

    public enum SensorType
    {
        PirSensor = 1 // Motion Sensor
    }
}
