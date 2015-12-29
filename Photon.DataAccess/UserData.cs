using Photon.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.DataAccess
{
    public class UserData
    {
        private static SqlConnection connection;

        /// <summary>
        /// Logs the start and finish occupancy time for the specified bathroom
        /// </summary>
        /// <param name="bathId"></param>
        /// <param name="occupiedTime"></param>
        /// <param name="freedTime"></param>
        public static void SaveUserConnection(User user)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kkcloudFreeDB"].ConnectionString;
            connection = new SqlConnection(connectionString);
            SqlTransaction transaction = null;

            try
            {
                SqlCommand command = connection.CreateCommand();

                if (connection.State != ConnectionState.Open)
                    connection.Open();
                //command.Parameters.AddWithValue("@bathId", bathroom.ID);
                //command.Parameters.AddWithValue("@occupiedTime", bathroom.LastOccupiedTime);
                
                transaction = connection.BeginTransaction();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SaveUserConnection";
                command.Transaction = transaction;

                foreach (var conn in user.Connections)
                {
                    if (conn.IsNew)
                    {
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@UserID", user.ID);
                        command.Parameters.AddWithValue("@ConnectionID", conn.ConnectionID);
                        command.Parameters.AddWithValue("@UserAgent", conn.UserAgent);
                        command.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                        command.Parameters.AddWithValue("@Connected", conn.Connected);

                        command.ExecuteNonQuery();                        
                    }

                   
                }
                //command.Parameters.AddWithValue("@freedTime", DateTime.Now);
             
                transaction.Commit();

                foreach (var conn in user.Connections)
                {
                    conn.IsNew = false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {
                if (connection != null)
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
            }
        }

        public static void UpdateUserConnection(string userID, Connection conn)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kkcloudFreeDB"].ConnectionString;
            connection = new SqlConnection(connectionString);
          
            try
            {
                SqlCommand command = connection.CreateCommand();

                if (connection.State != ConnectionState.Open)
                    connection.Open();
                //command.Parameters.AddWithValue("@bathId", bathroom.ID);
                //command.Parameters.AddWithValue("@occupiedTime", bathroom.LastOccupiedTime);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateUserConnection";

              
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@ConnectionID", conn.ConnectionID);
                command.Parameters.AddWithValue("@UserAgent", conn.UserAgent);
                command.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                command.Parameters.AddWithValue("@Connected", conn.Connected);

                command.ExecuteNonQuery();

                //command.Parameters.AddWithValue("@freedTime", DateTime.Now);              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
            }
        }
        public static List<User> GetUsersConnections()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kkcloudFreeDB"].ConnectionString;
            connection = new SqlConnection(connectionString);

            List<User> users = new List<User>();

            try
            {
                SqlCommand command = connection.CreateCommand();

                if (connection.State != ConnectionState.Open)
                    connection.Open();
                
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetUsersConnections";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    string userID = "0";
                    while (reader.Read())
                    {
                       
                        Connection conn = new Connection{
                            ConnectionID = reader.GetString(1),
                            UserAgent = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3),
                            Connected = reader.GetBoolean(4)
                        };

                        if (userID != reader.GetString(0))
                        {
                            User u = new User();
                            u.ID = reader.GetString(0);
                            users.Add(u);
                        }

                        users.Last().Connections.Add(conn);

                        userID = reader.GetString(0);
                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
            }

            return users;
        }
    }
}
