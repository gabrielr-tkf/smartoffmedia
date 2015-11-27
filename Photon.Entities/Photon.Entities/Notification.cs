using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class Notification
    {
        public User User { get; set; }
        public Bathroom Bathroom { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
