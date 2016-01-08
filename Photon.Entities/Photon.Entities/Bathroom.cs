using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class Bathroom
    {
        public Device PhotonDevice { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsOccupied { get; set; }

        public DateTime LastOccupiedTime { get; set; }
        public DateTime LastFreedTime { get; set; }

        public TimeSpan FreeFor
        {
            get
            {
                if (this.IsOccupied)
                {
                    return new TimeSpan(0);
                }
                else
                {
                    return DateTime.Now - this.LastFreedTime;
                }
            }

        }


    }
}
