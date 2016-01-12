using Photon.WebAPI.Classes;
using System.Web;
using System.Web.Mvc;

namespace Photon.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new ExceptionHandlerHelper());
        }
    }
}
