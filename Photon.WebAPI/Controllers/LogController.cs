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
                ((List<bool>)HttpContext.Current.Cache[Constants.OccupiedBaths])[bathId] = isOccupied;

                if (!isOccupied)
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Constants.kkcoudFreeDB].ConnectionString;
                    SqlConnection LoggingCN = new SqlConnection(connectionString);

                    DateTime occupiedTime = DateTime.Now;

                    occupiedTime = ((List<DateTime>)HttpContext.Current.Cache[Constants.LastOccupiedTimes])[bathId];

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
                    ((List<DateTime>)HttpContext.Current.Cache[Constants.LastOccupiedTimes])[bathId] = DateTime.Now;
                }


                if (HttpContext.Current.Cache["LogsTable"] == null)
                {
                    HttpContext.Current.Cache["LogsTable"] = new List<Tuple<int, bool, DateTime>>();
                    ((List<Tuple<int, bool, DateTime>>)HttpContext.Current.Cache["LogsTable"]).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
                }
                else
                {
                    ((List<Tuple<int, bool, DateTime>>)HttpContext.Current.Cache["LogsTable"]).Add(Tuple.Create(bathId, isOccupied, DateTime.Now));
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