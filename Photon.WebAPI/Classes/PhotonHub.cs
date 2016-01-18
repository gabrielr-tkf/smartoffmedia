using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using Photon.Entities;
using Photon.WebAPI.Utilities;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Photon.WebAPI.Classes
{
    public class PhotonHub : Hub
    {
        //public static void SendMessage(string user, BathStatus bathStatus)
        public static void SendMessage(Notification notification)
        {
            // Old School

            //List<User> UsersList = CacheManager.Get(Constants.UsersList) as List<User>;

            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<PhotonHub>();

            ////hubContext.Clients.All.broadcastMessage(bathid, state);
            //hubContext.Clients.Client(UsersList.Find(a => a.ID == notification.User.ID).ID).sendMessage(notification);

            //// TODO: These 2 lines are only for debugging purposes. They have to be removed
            //string stringNotification = notification.User.ID.Split('-')[0] + ": " + notification.Message;
            //((List<string>)System.Web.HttpRuntime.Cache["Notifications"]).Add(stringNotification);


            // New School

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PhotonHub>();

            User user = Services.User.Find(notification.User.ID);
            List<Connection> connections = user.Connections.Where(c => c.Connected).ToList();

            if (connections.Count > 0)
            {
                foreach (Connection conn in connections)
                {
                    hubContext.Clients.Client(conn.ConnectionID).sendMessage(notification);
                }

                // TODO: These 2 lines are only for debugging purposes. They have to be removed
                string stringNotification = notification.User.ID.Split('-')[0] + ": " + notification.Message;
                ((List<string>)System.Web.HttpRuntime.Cache["Notifications"]).Add(stringNotification);
            }

           //hubContext.Clients.Client(UsersList.Find(a => a.ID == notification.User.ID).ID).sendMessage(notification);

            
        }

        public override Task OnConnected()
        {
            //var name = HttpContext.Current.Request.QueryString["localIP"];

            //var user = Services.User.Find(name);

            //if (user == null)
            //{
            //    user = new User
            //    {
            //        ID = name,
            //    };
            //    //db.Users.Add(user);
            //}

            //user.Connections.Add(new Connection
            //{
            //    ConnectionID = Context.ConnectionId,
            //    UserAgent = Context.Request.Headers["User-Agent"],
            //    Connected = true,
            //    IsNew = true
            //});
            //Services.User.SaveUserConnection(user);

          
            this.registerConId();
          
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //var connection = db.Connections.Find(Context.ConnectionId);
            //connection.Connected = false;
            //db.SaveChanges();

            //TODO: Code Refactoring
            User user = Services.User.FindByConnectionID(Context.ConnectionId);
            foreach (Connection c in user.Connections)
            {

                if (c.ConnectionID == Context.ConnectionId)
                {
                    c.Connected = false;

                }

                Services.User.UpdateUserConnection(user.ID, c);
            }

            return base.OnDisconnected(stopCalled);
        }
      
        //public static List<string> UsersConnectionIds = new List<string>();
        //public static List<User> UsersList = new List<User>();
        public string registerConId()
        {
            //List<User> UsersList = CacheManager.Get(Constants.UsersList) as List<User>;
            //if (UsersList.Count == 0)
            //{
            //    UsersList.Add(new User()
            //    {
            //        ID = Context.User.Identity.Name
            //    });
            //}
            //else
            //{
            //    if (!UsersList.Exists(a => a.ID == Context.ConnectionId))
            //    {
            //        UsersList.Add(new User()
            //        {
            //            ID = Context.ConnectionId
            //        });
            //    }
            //}
            //return Context.ConnectionId;

            var name = HttpContext.Current.Request.QueryString["localIP"];

            var user = Services.User.Find(name);

            if (user == null)
            {
                user = new User
                {
                    ID = name,
                };
                //db.Users.Add(user);
            }
            bool addConnection = false;
            if (user.Connections.Count > 0)
            {
                int connectionCount = user.Connections.Where(a => a.ConnectionID == Context.ConnectionId && a.Connected == true).Count();
                if (connectionCount == 0)
                {
                    //New connection
                    addConnection = true;
                }
            }else
            {
                //New connection => 1st Connection
                addConnection = true;
            }

            if (addConnection)
            {
                user.Connections.Add(new Connection
                {
                    ConnectionID = Context.ConnectionId,
                    UserAgent = Context.Request.Headers["User-Agent"],
                    Connected = true,
                    IsNew = true
                });
                Services.User.SaveUserConnection(user);
            }

          

            return Context.ConnectionId;
        }
    }
}