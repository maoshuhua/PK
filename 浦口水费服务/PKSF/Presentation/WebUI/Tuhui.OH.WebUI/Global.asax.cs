using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tuhui.Common.Mvc;
using Tuhui.Common.Environment;

using System.Web.Security;

namespace Tuhui.OH.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : BaseHttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoggerFilter());
            filters.Add(new ExceptionFilter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "API", // Route name
                "api", // URL with parameters
                new { controller = "APIMock", action = "Index" } // Parameter defaults
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Home", action = "Index" },// Parameter defaults
                new string[] { "Tuhui.OH.WebUI.Controllers" }
            );


        }

        protected void Application_Start()
        {
            EnvironmentHandle.Register();
            EnvironmentHandle.RegisterLanguageHandle(new DefaultLanguageHandle());
            EnvironmentHandle.RegisterMessageLoadHandle(new DefaultMessageLoadHandle());
            THCommHandler.Register();
            APIRouteHelper.Register();
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            var ex = Server.GetLastError();

            var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误
            var httpContext = ((MvcApplication)sender).Context;
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = httpStatusCode;
            var shouldHandleException = true;
            HandleErrorInfo errorModel;

            var routeData = new RouteData();
            routeData.Values["controller"] = "Utility";
            routeData.Values["action"] = "Error";

            if (ex is HttpRequestValidationException)
            {
                Response.Redirect("~/GenericError.html");
                return;
            }

            switch (httpStatusCode)
            {
                case 404:
                    
                    //errorModel = new HandleErrorInfo(new Exception(string.Format("No page Found", httpContext.Request.UrlReferrer), ex), "Utility", "Error");
                    break;

                default:

                    Exception exceptionToReplace = null; 
                    //errorModel = new HandleErrorInfo(new Exception(string.Format("Error", httpContext.Request.UrlReferrer), ex), "Utility", "Error");

                    break;
            }

            //if (shouldHandleException)
            //{
            //    var controller = new UtilityController();
            //    controller.ViewData.Model = errorModel; //通过代码路由到指定的路径
            //    ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            //}
        }



    }
}