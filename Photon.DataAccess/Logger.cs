using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photon.DataAccess
{
    public class Logger
    {
        /// <summary>
        /// Logs the start and finish occupancy time for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="occupiedTime"></param>
        /// <param name="freedTime"></param>
        public void LogBathUsage(int bathId, DateTime occupiedTime, DateTime freedTime)
        {

        }

        /// <summary>
        /// Logs the detection of methane gas for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        public void LogCH4Detected(int bathId)
        {

        }
    }
}
