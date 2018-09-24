using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using ParkingQueue.Service;

[assembly: OwinStartup(typeof(ParkingQueue.Service.Startup))]
namespace ParkingQueue.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR();
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR("/signalr", new HubConfiguration() { EnableJSONP = true, EnableJavaScriptProxies = true });
        }
    }
}