using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingQueue.Service;
using ParkingQueue.Service.Controllers;
using Newtonsoft.Json;
using ParkingQueue.Model;

namespace ParkingQueue.Service.Tests.Controllers
{
    [TestClass]
    public class LogControllerTest
    {
        [TestMethod]
        public void LogControllerPutTest()
        {
            try
            {
                var controller = new LogController();
                controller.Put("TestException", new Exception("TestException"));
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
