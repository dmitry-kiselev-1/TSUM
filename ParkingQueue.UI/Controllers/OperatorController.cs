using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class OperatorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Интерфейс Оператора Электронной Очереди";

            return View();
        }
    }
}
