using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Photon.WebAPI;
using System.Web.Routing;

namespace Photon.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            app.MapSignalR();
        }
    }
}