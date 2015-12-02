using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class NotificationGetNotificationStatusByUserResponse : BaseResponse
    {
        public bool SubscribedNotificationBath1 { get; set; }
        public bool SubscribedNotificationBath2 { get; set; }
        public bool SubscribedNotificationBath3 { get; set; }

    }
}