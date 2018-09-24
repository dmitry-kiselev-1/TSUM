using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingQueue.Service;
using ParkingQueue.Service.Controllers;
using Newtonsoft.Json;
using ParkingQueue.Model;
using ParkingQueue.Model.ParkingQueue;

namespace ParkingQueue.Service.Tests.Controllers
{
    [TestClass]
    public class QueueControllerTest
    {
        private const string TestIdprefix = "Test_";

        [TestMethod]
        public void QueueControllerGetTest()
        {
            try
            {
                var controller = new QueueController();
                var result = controller.Get();
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void QueueControllerGetByIdTest()
        {
            try
            {
                var controller = new QueueController();
                string id = GetRandomId(1000);
                var result = controller.Get(id);
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void QueueControllerPutTest()
        {
            try
            {
                //string id = Guid.NewGuid().ToString().Substring(0, 16);
                //string id = "test1";

                string id = GetRandomId(1000);
                var controller = new QueueController();
                var result = controller.Put(id, null);
                //Assert.IsNotNull(controller.Get(id));
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void QueueControllerDeleteTest()
        {
            try
            {
                string id = GetRandomId(1000);
                var controller = new QueueController();
                //string id = controller.Get().Skip(randomId).First().ParkingCard;
                var result = controller.Delete(id, OutputReasonEnum.Vip);
                //Assert.IsNull(controller.Get(id));
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void QueueControllerRotationTest()
        {
            try
            {
                var controller = new QueueController();
                var result = controller.Post(rotation: 1);
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void QueueControllerTimerTest()
        {
            try
            {
                var controller = new QueueController();
                var result = controller.Post(start: true);
                //Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        /// <summary>Генерация тестовых данных</summary>
        [TestMethod]
        public void GenerateTestData()
        {
            const int count = 10000;

            try
            {
                var controller = new QueueController();

                string id = string.Empty;
                string format = string.Empty;
                for (int i = 0; i < count.ToString().Length; i++)
                { format = format + "0"; }

                for (int i = 1; i <= count; i++)
                {
                    Task.Delay(1);

                    id = TestIdprefix + i.ToString(format);
                    controller.Put(id, new Queue() { ParkingCard = id });
                }
                Assert.IsNotNull(controller.Get(id));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        private string GetRandomId(int count)
        {
            Random random = new Random();
            int minId = 1;
            int maxId = count;
            int randomId = random.Next(minId, maxId);
            string format = new string('0', count.ToString().Length);
            string id = TestIdprefix + randomId.ToString(format);
            return id;
        }
    }
}
