using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class NotificationSubscriptionStatusByUserResponse : BaseResponse
    {
        public List<Photon.Entities.UserSubscriptionStatus> UserSubscriptionStatusList { get; set; }
    }
}