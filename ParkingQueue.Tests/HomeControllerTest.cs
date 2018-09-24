using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingQueue.Service;
using ParkingQueue.Service.Controllers;

namespace ParkingQueue.Service.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            UIController controller = new UIController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
