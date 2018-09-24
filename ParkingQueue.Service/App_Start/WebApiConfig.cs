using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ParkingQueue.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Запрет сериализации в XML:
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            // Web API configuration and services

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
            config.Routes.MapHttpRoute(
                name: "ApiQueue",
                routeTemplate: "api/{id}",
                defaults: new { controller = "Queue", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ApiOperator",
                routeTemplate: "operator-api/{id}",
                defaults: new { controller = "Operator", id = RouteParameter.Optional }
            );
            */
        }
    }
}
