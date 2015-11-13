using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class DeviceGetResponse : BaseResponse
    {
        public Photon.Entities.Device Device { get; set; }
    }
}