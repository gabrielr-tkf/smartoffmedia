using Photon.DataAccess;
using Photon.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Services
{
    public class User
    {
        public static void SaveUserConnection(Entities.User user)
        {
            UserData.SaveUserConnection(user);
        }

        public static void UpdateUserConnection(string userID, Entities.Connection conn)
        {
            UserData.UpdateUserConnection(userID, conn);
        }

        private static List<Entities.User> GetUsersConnections()
        {
            if (CacheManager.ValidatExistence("UsersConnections"))
            {
                return CacheManager.Get("UsersConnections") as List<Entities.User>;
            }
            else
            {
                CacheManager.Add("UsersConnections", UserData.GetUsersConnections());
                return CacheManager.Get("UsersConnections") as List<Entities.User>;
            }
        }

        public static Entities.User Find(string userID)
        {
            return GetUsersConnections().Where(u => u.ID == userID).FirstOrDefault();
        }

        public static Entities.Connection FindConnected(string userID)
        {
            Entities.User user = GetUsersConnections().Where(u => u.ID == userID).FirstOrDefault();
            if(user != null){
                return user.Connections.Where(c => c.Connected).FirstOrDefault();
            }

            return null;
        }

        public static Entities.User FindByConnectionID(string connectionID)
        {
            return GetUsersConnections().Where(u => u.Connections.Where(c => c.ConnectionID == connectionID).FirstOrDefault() != null).FirstOrDefault();
        }

    }
}
