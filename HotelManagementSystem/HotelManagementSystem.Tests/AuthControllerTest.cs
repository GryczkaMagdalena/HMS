using System;
using HotelManagementSystem.Controllers;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Xunit;
using HotelManagementSystem.Tests.Infrastructure;
using System.Collections.Generic;
using HotelManagementSystem.Models.Entities.Storage;

namespace HotelManagementSystem.Tests
{
   public class AuthControllerTest
    {
        private readonly IUserService _userService;
        private readonly IdentityContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthControllerTest()
        {
            _context = new IdentityContext();
            _logger = new MockLogger<AuthController>();
            _userService = new MockUserService();
        }

        [Theory]
        [InlineData("guest1", "Gue$t1")]
        [InlineData("worker1", "Wor<er1")]
        [InlineData("worker5", "Wor<er5")]
        public void GetToken(string login, string password)
        {
            var controller = new AuthController(_userService, _context, _logger);
            IActionResult response =  controller.Token(new Models.Concrete.UserViewModel()
            {
                Login=login,
                Password=password
            }).Result;

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;
            dynamic jsonObject = new DynamicObjectResultValue(result.Value);
            Assert.NotNull(jsonObject.token);
            Assert.NotNull(jsonObject.user);
            dynamic userObject = new DynamicObjectResultValue(jsonObject.user);
            Assert.NotEmpty(userObject.Roles);
        }
    }
}