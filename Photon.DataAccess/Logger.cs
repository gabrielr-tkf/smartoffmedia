using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Photon.DataAccess
{
    public class Logger
    {
        private static SqlConnection LoggingCN;

        /// <summary>
        /// Logs the start and finish occupancy time for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="occupiedTime"></param>
        /// <param name="freedTime"></param>
        public static void LogBathUsage(Bathroom bathroom)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kkcloudFreeDB"].ConnectionString;
            LoggingCN = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = LoggingCN.CreateCommand();

                if (LoggingCN.State != ConnectionState.Open)
                    LoggingCN.Open();
                command.Parameters.AddWithValue("@bathId", bathroom.ID);
                command.Parameters.AddWithValue("@occupiedTime", bathroom.LastOccupiedTime);
                command.Parameters.AddWithValue("@freedTime", DateTime.Now);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "LogBathUsage";
                command.ExecuteScalar();
            }
            catch (Exception ex) {
                throw ex;
            }
            finally
            {
                if (LoggingCN != null)
                    if (LoggingCN.State != ConnectionState.Closed)
                        LoggingCN.Close();
            }
        }

        /// <summary>
        /// Logs the start and finish occupancy time for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="occupiedTime"></param>
        /// <param name="freedTime"></param>
        public static void LogDeviceReport(string deviceId, string sensorId, string value)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kkcloudFreeDB"].ConnectionString;
            LoggingCN = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = LoggingCN.CreateCommand();

                if (LoggingCN.State != ConnectionState.Open)
                    LoggingCN.Open();
                command.Parameters.AddWithValue("@DeviceId", deviceId);
                command.Parameters.AddWithValue("@SensorId", int.Parse(sensorId));
                command.Parameters.AddWithValue("@Value", int.Parse(value));
                command.Parameters.AddWithValue("@Time", DateTime.Now);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "LogDeviceReport";
                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (LoggingCN != null)
                    if (LoggingCN.State != ConnectionState.Closed)
                        LoggingCN.Close();
            }
        }

        /// <summary>
        /// Logs the detection of methane gas for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        public static void LogCH4Detected(int bathId)
        {

        }
    }
}
