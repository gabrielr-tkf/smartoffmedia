﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Entities
{
    public class NotificationUnsubscribeResponse : BaseResponse
    {
        public string UserId { get; set; }
        public string BathId { get; set; }
        public string NotificationTitle  { get; set; }
        public string NotificationMessage { get; set; }

    }
}