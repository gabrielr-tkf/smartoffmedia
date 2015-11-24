using Microsoft.AspNet.SignalR;
using Photon.WebAPI.Classes;
using Photon.WebAPI.Entities;
using Photon.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace Photon.WebAPI.Controllers
{
    public class LogController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET")]
        public LogStateChangeResponse LogStateChange(int bathId, bool isOccupied)
        {
            LogStateChangeResponse response = new LogStateChangeResponse();

            try
            {
                (CacheManager.Get(Constants.OccupiedBaths) as List<bool>)[bathId - 1] = isOccupied;

                if (!isOccupied)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Constants.kkcoudFreeDB].ConnectionString;
                    SqlConnection LoggingCN = new SqlConnection(connectionString);

                    DateTime occupiedTime = DateTime.Now;

                    occupiedTime = (CacheManager.Get(Constants.LastOccupiedTimes) as List<DateTime>)[bathId - 1];

                    try
                    {
                        SqlCommand command = LoggingCN.CreateCommand();

                        if (LoggingCN.State != ConnectionState.Open)
                            LoggingCN.Open();
                        command.Parameters.AddWithValue("@bathId", bathId);
                        command.Parameters.AddWithValue("@occupiedTime", occupiedTime);
                        command.Parameters.AddWithValue("@freedTime", DateTime.Now);
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "LogBathUsage";
                        command.ExecuteScalar();
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (LoggingCN != null)
                            if (LoggingCN.State != ConnectionState.Closed)
                                LoggingCN.Close();
                    }

                    NotificationController notificationController = new NotificationController();
                    notificationController.Publish(bathId, isOccupied);
                }
                else
                {
                    (CacheManager.Get(Constants.LastOccupiedTimes) as List<DateTime>)[bathId] = DateTime.Now;
                }


                if (!CacheManager.ValidatExistence("LogsTable"))
                {
                    CacheManager.Add("LogsTable", new List<Tuple<int, bool, DateTime>>());
                    (CacheManager.Get("LogsTable") as List<Tuple<int, bool, DateTime>>).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
                }
                else
                {
                    (CacheManager.Get("LogsTable") as List<Tuple<int, bool, DateTime>>).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
                }

                response.Status = "200";
                response.Message = "State successfully logged";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error";
            }

            return response;
        }
    }
}