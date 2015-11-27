using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Services
{
    public class Logger
    {
        public static void LogBathUsage(Bathroom bathroom)
        {
            Photon.DataAccess.Logger.LogBathUsage(bathroom);
        }
    }
}
