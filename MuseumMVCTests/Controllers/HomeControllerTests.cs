using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuseumMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;

namespace MuseumMVC.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void LogOutTest()
        {
            //Arrange
            HomeController controller = new HomeController();
            //Act
            ViewResult result = controller.Index() as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            HomeController controller = new HomeController();
            //Act
            ViewResult result = controller.Index() as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }
    }
}