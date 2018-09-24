using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ParkingQueue.Model;

namespace ParkingQueue.Service
{
    public class QueueHub : Hub
    {
        public void Example(string prm)
        {
            Clients.All.example(prm);
        }
    }
}