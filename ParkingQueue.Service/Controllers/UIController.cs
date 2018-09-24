using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkingQueue.Model;

namespace ParkingQueue.Service.Controllers
{
    public class UIController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Статус электронной очереди";

            return View();
        }

        public ActionResult Client()
        {
            ViewBag.Title = "Табло электронной очереди";

            return View();
        }
    }
}
