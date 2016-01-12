using Photon.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Photon.WebAPI.Classes
{
    public class ExceptionHandlerHelper : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            
            
            //Log Critical errors
            Debug.WriteLine(context.Exception);

            Logger.LogError(context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                context.ActionContext.ActionDescriptor.ActionName,
                context.Exception,
               HttpContext.Current.Request.UserHostAddress,
               context.ActionContext.Request.RequestUri.AbsoluteUri);

                

           
        }
    }
}