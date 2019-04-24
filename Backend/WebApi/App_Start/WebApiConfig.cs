using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS globaly across all controllers
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");

            // Web API configuration and services
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                              "text/html",
                              StringComparison.InvariantCultureIgnoreCase,
                              true,
                              "application/json"));
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "LoginRoute",
                routeTemplate: "login",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: new LoginHandler()
            );

            config.Routes.MapHttpRoute(
                name: "DeleteUserRoute",
                routeTemplate: "deleteuser",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: new DeleteHandler()
            );

            config.Routes.MapHttpRoute(
                name: "HealthCheckRoute",
                routeTemplate: "health",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: new HealthHandler()
            );
        }
    }
}
