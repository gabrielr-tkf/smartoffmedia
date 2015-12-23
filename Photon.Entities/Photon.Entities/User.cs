using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class User
    {
        public User()
        {
            Connections = new List<Connection>();
        }
        public string ID { get; set; }
        public List<Connection> Connections { get; set; }

    }
}
