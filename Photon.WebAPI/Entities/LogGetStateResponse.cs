using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class LogGetStateResponse : BaseResponse
    {
        public List<Photon.Entities.BathStatus> BathStatus { get; set; }
    }
}