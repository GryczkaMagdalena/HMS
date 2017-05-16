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
namespace HMS.Tests
{
    [TestClass]
    public class RoomTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
       
        [TestMethod]
        public async System.Threading.Tasks.Task CreateRoom()
        {
            var room = new Room();
            Assert.IsNotNull(room);
        }
    }
}
