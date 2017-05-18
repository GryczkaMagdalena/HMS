using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagementSystem.Controllers;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Entities.Storage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;
using HMS.Tests.Infrastructure;
using System.Collections.Generic;

namespace HMS.Tests
{
    [TestClass]
    public class CaseControllerTest
    {
        private IdentityContext _context;
        private UserService _userService;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        ///
        [TestInitialize]
        public void Setup()
        {
            _context = new IdentityContext();
        }
        
       
        [TestMethod]
        public async System.Threading.Tasks.Task AnyCaseExists()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger,_context);
            IActionResult rawResponse = await controller.List();
            Assert.IsNotNull(rawResponse);
            Assert.IsInstanceOfType(rawResponse, typeof(OkObjectResult));
            var result = rawResponse as OkObjectResult;
            dynamic jsonCollection = result.Value;
            foreach(dynamic value in jsonCollection)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.IsNotNull(json);
                Assert.IsNotNull(json.CaseID);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task MoreThan4Cases()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger, _context);
            IActionResult rawResponse = await controller.List();
            Assert.IsNotNull(rawResponse);
            Assert.IsInstanceOfType(rawResponse, typeof(OkObjectResult));
            var result = rawResponse as OkObjectResult;
            dynamic jsonColletion = result.Value;
            int counter = 0;
            foreach(dynamic value in jsonColletion)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.IsNotNull(json);
                Assert.IsNotNull(json.CaseID);
                counter++;
            }

            Assert.IsTrue(counter >= 4);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task GetTaskByID()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger, _context);
            IActionResult rawResponse = await controller.List();
            Assert.IsNotNull(rawResponse);
            Assert.IsInstanceOfType(rawResponse, typeof(OkObjectResult));
            var result = rawResponse as OkObjectResult;
            dynamic jsonColletion = result.Value;
            foreach (dynamic value in jsonColletion)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.IsNotNull(json);
                Assert.IsNotNull(json.CaseID);
                var caseID = json.CaseID;
                IActionResult rawGet= await controller.Read(caseID);
                Assert.IsNotNull(rawGet);
                Assert.IsInstanceOfType(rawGet, typeof(OkObjectResult));
                var getResult = rawGet as OkObjectResult;
                dynamic getResponse = new DynamicObjectResultValue(getResult.Value);
                Assert.AreEqual(caseID, getResponse.CaseID);
            }
        }

    }

}
