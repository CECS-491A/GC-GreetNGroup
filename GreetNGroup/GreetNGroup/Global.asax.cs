using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

// Global.asax acts as a class repressenting the application as a whole
namespace GreetNGroup
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /*
         Function still in consideration for adding -- application specific but useful for generic missed
         exception catch

            Will currently use this function as a last resort error catch, it is non-descriptive an wide catching
        void Application_Error(object s, EventArgs e)
        {
            var httpEx = Server.GetLastError() as HttpException;

            if (httpEx != null)
            {
                if (httpEx.GetHttpCode() == 404)
                {
                    Response.StatusCode = 404;
                }
            }
        }
        */
    }
}
