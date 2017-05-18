using System;
using HotelManagementSystem.Controllers;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Xunit;
using HMS.Tests.Infrastructure;
using System.Collections.Generic;

namespace HMS.Tests
{
    
    public class CaseControllerTest
    {
        private IdentityContext _context;
        private UserService _userService;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        ///
        
        public CaseControllerTest()
        {
            _context = new IdentityContext();
        }
        
       
        [Fact]
        public async System.Threading.Tasks.Task AnyCaseExists()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger,_context);
            IActionResult rawResponse = await controller.List();
            Assert.NotNull(rawResponse);
            Assert.IsType<OkObjectResult>(rawResponse);
            var result = rawResponse as OkObjectResult;
            dynamic jsonCollection = result.Value;
            foreach(dynamic value in jsonCollection)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.NotNull(json);
                Assert.NotNull(json.CaseID);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task MoreThan4Cases()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger, _context);
            IActionResult rawResponse = await controller.List();
            Assert.NotNull(rawResponse);
            Assert.IsType<OkObjectResult>(rawResponse);
            var result = rawResponse as OkObjectResult;
            dynamic jsonColletion = result.Value;
            int counter = 0;
            foreach(dynamic value in jsonColletion)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.NotNull(json);
                Assert.NotNull(json.CaseID);
                counter++;
            }

            Assert.True(counter >= 4);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByID()
        {
            var logger = new MockLogger<CaseController>();
            var controller = new CaseController(logger, _context);
            IActionResult rawResponse = await controller.List();
            Assert.NotNull(rawResponse);
            Assert.IsType<OkObjectResult>(rawResponse);
            var result = rawResponse as OkObjectResult;
            dynamic jsonColletion = result.Value;
            foreach (dynamic value in jsonColletion)
            {
                dynamic json = new DynamicObjectResultValue(value);
                Assert.NotNull(json);
                Assert.NotNull(json.CaseID);
                var caseID = json.CaseID;
                IActionResult rawGet= await controller.Read(caseID);
                Assert.NotNull(rawGet);
                Assert.IsType<OkObjectResult>(rawGet);
                var getResult = rawGet as OkObjectResult;
                dynamic getResponse = new DynamicObjectResultValue(getResult.Value);
                Assert.Equal(caseID, getResponse.CaseID);
            }
        }

    }

}
