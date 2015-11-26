using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class BathStateResponse : BaseResponse
    {
        public List<Photon.Entities.BathStatus> BathStatusList { get; set; }
    }
}