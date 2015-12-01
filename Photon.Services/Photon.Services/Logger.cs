using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Photon.Services
{
    public class Logger
    {
        public static void LogBathUsage(Bathroom bathroom)
        {
            new Thread(() =>
            {
                Photon.DataAccess.Logger.LogBathUsage(bathroom);
            }).Start();            
        }
    }
}
