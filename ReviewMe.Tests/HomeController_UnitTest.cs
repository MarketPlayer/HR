using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewMe.Controllers;

namespace ReviewMe.Tests
{
    [TestClass]
    public class HomeController_UnitTest
    {
        [TestMethod]
        public void HomeController_GetVisitorsCount()
        {
            HomeController homeController = new HomeController();

            string storeName = "player1";

            int initialVisitorsCount = homeController.GetVisitorsCount(storeName).Result;

            int finalVisitorsCount = homeController.GetVisitorsCount(storeName).Result;

            Assert.AreEqual<int>(initialVisitorsCount, finalVisitorsCount);
        }
    }
}
