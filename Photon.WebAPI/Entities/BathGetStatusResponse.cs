using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class BathGetStatusResponse : BaseResponse
    {
        public List<Photon.Entities.BathroomLine> BathStatusList { get; set; }
    }
}