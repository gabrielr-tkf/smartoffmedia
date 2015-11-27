using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class Bathroom
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsOccupied { get; set; }

        public DateTime LastOccupiedTime { get; set; }
        public DateTime LastFreedTime { get; set; }


    }
}
