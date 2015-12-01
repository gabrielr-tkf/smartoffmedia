using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Entities
{
    public class BathroomLine
    {
        public Bathroom Bathroom { get; set; }
        public List<User> UsersLine { get; set; }
        public DateTime LastTimesFirstChanged { get; set; }
        public BathroomLine()
        {
            this.UsersLine = new List<User>();
        }

        //
    }
}
