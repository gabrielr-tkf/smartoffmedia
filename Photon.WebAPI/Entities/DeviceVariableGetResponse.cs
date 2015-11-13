using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class DeviceVariableGetResponse : BaseResponse
    {
        public Photon.Entities.Variable Variable { get; set; }
    }
}