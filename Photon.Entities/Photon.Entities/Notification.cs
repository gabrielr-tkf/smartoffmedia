using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class Notification
    {
        public Notification()
        {
            Type = NotificationType.WITHOUT_BUTTON;
        }

        public User User { get; set; }
        public Bathroom Bathroom { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string AudioFile { get; set; }
        public NotificationType Type { get; set; }

        
    }

    public enum NotificationType
    {
        WITH_BUTTON = 1,
        WITHOUT_BUTTON = 2
    }
}
