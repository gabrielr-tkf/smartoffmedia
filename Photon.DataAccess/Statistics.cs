using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photon.DataAccess
{
    public class Statistics
    {
        /// <summary>
        /// Returns the average occupation time per use
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetAverageTimePerUse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the average occupation time per use for the specified day
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public TimeSpan GetAverageTimePerUse(DateTime day)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the total occupation time per day
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalOccupiedTimePerDay()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the total time for the specified day
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public TimeSpan GetTotalOccupiedTimePerDay(DateTime day)
        {
            throw new NotImplementedException();
        }
    }
}
