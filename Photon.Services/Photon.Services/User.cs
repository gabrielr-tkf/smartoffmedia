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
            return UserData.GetUsersConnections();
        }

        public static Entities.User Find(string userID)
        {
            List<Entities.User> list = GetUsersConnections();
            Entities.User user = list.Where(u => u.ID == userID).FirstOrDefault();
            return user;
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
            List<Entities.User> usersAux = GetUsersConnections();

            List<Entities.User> users = (from u in usersAux
                            where u.Connections.Exists(c=> c.ConnectionID == connectionID)
                            select u).ToList();

            return users.First();
        }

    }
}
