using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class NotificationSubscribeResponse : BaseResponse
    {
        public string UserId { get; set; }
        public string BathId { get; set; }
    }
}