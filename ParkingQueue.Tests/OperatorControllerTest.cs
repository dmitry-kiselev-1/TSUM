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
    public class OperatorControllerTest
    {
        [TestMethod]
        public void Get()
        {
            try
            {
                var controller = new OperatorController();
                var result = controller.Get();
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetById()
        {
            try
            {
                var controller = new OperatorController();
                var result = controller.Get("login_1");
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Put()
        {
            try
            {
                var controller = new OperatorController();

                string id = Guid.NewGuid().ToString().Substring(0, 16);

                controller.Put(id, new Operator()
                {
                    Login = id,
                    IsAdmin = false,
                    Name = "Name_" + id
                });

                Assert.IsNotNull(controller.Get(id));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Delete()
        {
            try
            {
                string id = "login_2";

                var controller = new OperatorController();
                controller.Delete(id);

                Assert.IsNull(controller.Get(id));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
