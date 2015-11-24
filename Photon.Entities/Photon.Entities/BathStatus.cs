using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class BathStatus
    {
        public int BathId { get; set; }
        public bool IsOccupied { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

    }
}
