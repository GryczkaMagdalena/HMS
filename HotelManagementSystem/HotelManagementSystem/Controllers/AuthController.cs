using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models.Concrete;
using Newtonsoft.Json;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Auth/[action]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private StorageContext db = new StorageContext();
        private IdentityContext idb = new IdentityContext();
        private readonly string externalCookieScheme;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            RoleManager<Role> roleManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.Authentication.SignOutAsync(externalCookieScheme);
            return Json(new { status="clear" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.Login, user.Password, true, false);
                if (result.Succeeded)
                {
                    var entity = idb.Users.First(q => q.UserName == user.Login);
                    return Json(new { entity });
                }
                else
                {
                    return Json(new { status = "unauthorized" });
                }
            }
            else
            {
                return Json(new { status = "fail" });
            }
        }

        [HttpPost]
        public  async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Json(new {status="logout" });
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerModel) 
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = registerModel.Login,
                    PhoneNumber =registerModel.PhoneNumber,
                    LastName =registerModel.LastName,
                    FirstName =registerModel.FirstName};
                var result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, registerModel.RoleName);
                    if (roleResult.Succeeded)
                    {
                        return Json(new { status = "registered" });
                    }
                }
            }
            return Json(new { status = "failure" });
        }
    }
}