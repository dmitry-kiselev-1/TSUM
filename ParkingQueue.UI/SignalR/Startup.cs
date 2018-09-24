using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using ParkingQueue.UI;

[assembly: OwinStartup(typeof(ParkingQueue.UI.Startup))]
namespace ParkingQueue.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //app.MapSignalR("/signalr", new HubConfiguration(){ EnableJSONP = true });
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR("/signalr", new HubConfiguration() { EnableJSONP = true, EnableJavaScriptProxies = true });
            //app.MapSignalR("/localhost:81/signalr", new HubConfiguration() { EnableJSONP = true, EnableJavaScriptProxies = true});
        }
    }
}