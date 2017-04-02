using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models.Infrastructure;

namespace HotelManagementSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private StorageContext db = new StorageContext();
        private IdentityContext idb = new IdentityContext();
        public  ActionResult GetUsers()
        {
            var users = idb.Users.ToArray();
            return Json(new { users } );
        }
    }
}