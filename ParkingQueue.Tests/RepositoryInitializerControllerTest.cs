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


namespace ParkingQueue.Service.Tests.Controllers
{
    [TestClass]
    public class RepositoryInitializerControllerTest
    {
        [TestMethod]
        public void Get()
        {
            try
            {
                var controller = new RepositoryInitializerController();
                controller.Get(withTestData: "true");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
