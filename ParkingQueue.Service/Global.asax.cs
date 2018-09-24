using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ParkingQueue.Model;
using ParkingQueue.Model.ParkingQueue;
using ParkingQueue.Service.Controllers;
using WebApiContrib.Formatting.Jsonp;

namespace ParkingQueue.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // поддержка jsonp:
            GlobalConfiguration.Configuration.AddJsonpFormatter();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // глобальный обработчик ошибок:
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Model.Log.Put(
                    message: ((Exception) args.ExceptionObject).Message,
                    exception: (Exception) args.ExceptionObject);
            };

            // запуск таймера:
            bool startTimer;
            bool.TryParse(ConfigurationManager.AppSettings["StartTimer"], out startTimer);
            new QueueController().Post(start: startTimer);

            // запуск push-уведомлений:
            Model.Notifier.QueueChanged += (sender, args) =>
            {
                string reason = ((QueueChangedEventArgs)args).Reason.ToString();
                var queue = ((QueueChangedEventArgs)args).Queue;
                int rotationTimeLimitParam = ((QueueChangedEventArgs)args).RotationTimeLimitParam;

                var queueFormatted = queue.Select(t => new
                                                       {
                                                           t.Position,
                                                           t.ParkingCard,
                                                           t.Rotation,
                                                           Output = t.Output?.ToShortTimeString()
                                                       }).ToList();

                string queueString = Newtonsoft.Json.JsonConvert.SerializeObject(
                    queueFormatted, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                        /*NullValueHandling = NullValueHandling.Ignore*/
                    });
                
                //int rotationTimeLimitParam = int.Parse(new Parameter().Get(ParameterEnum.RotationTimeLimitParam.ToString()).Value);

                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                context.Clients.All.queueChanged(queueString, reason, rotationTimeLimitParam);
            };
        }
    }
}
