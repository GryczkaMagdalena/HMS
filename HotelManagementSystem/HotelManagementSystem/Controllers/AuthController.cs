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
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace HotelManagementSystem.Controllers
{

    [EnableCors("HotelCorsPolicy")]
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
        /**
            * @api {get} /Auth/Login Login - clear cookie
            * @apiVersion 0.1.0
            * @apiName GetLogin
            * @apiGroup Auth
            *
            *
            * @apiSuccess {String} status Message about cleared cookie.
            *
            * @apiSuccessExample Success-Response:
            *     HTTP/1.1 200 OK
            *     {
            *       "status": "clear",
            *     }
        */
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.Authentication.SignOutAsync(externalCookieScheme);
            return Json(new { status = "clear" });
        }

        /**
          * @api {post} /Auth/Login Login
          * @apiVersion 0.1.0
          * @apiName PostLogin
          * @apiGroup Auth
          *
          * @apiParam {String} Login Email or login of user
          * @apiParam {String} Password User's password
          *
          * @apiSuccess {String} FirstName First name of user.
          * @apiSuccess {String} LastName Last name of user.
          * @apiSuccess {String} Email Optional email address of user
          * @apiSuccess {String} PhoneNumber Optional phone number of user
          * @apiSuccess {String} WorkerType One of available types (Cleaner,Technician,None).
          * @apiSuccess {Array} Role All roles that particular user have 
          * @apiSuccess {GUID} RoomID Optional parameter - only guests have this not-null 
          *
          * @apiSuccessExample Success-Response:
          *     HTTP/1.1 200 OK
          *     {
          *     "firstName":"Abraham",
          *     "lastName":"Lincoln",
          *     "email":"president@usa.pl",
          *     "phoneNumber":"123-908-123",
          *     "workerType":"Technician",
          *     "role":["Worker"]
          *     }
          *@apiError MissingData Login or Password are missing.
          * @apiErrorExample Error-Response:
          * HTTP/1.1 200 OK
          * {
          * "status":"fail"
          * }
          * 
          * @apiError Unathorized This User does not exist or password is invalid.
          * @apiErrorExample Error-Response:
          * HTTP/1.1 200 OK
          * {
          * "status":"unauthorized"
          * }
      */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.Login, user.Password, true, false);
                if (result.Succeeded)
                {
                    var entity = idb.Users.First(q => q.UserName == user.Login);
                    var roles = await userManager.GetRolesAsync(entity);

                    var output = new
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Email = entity.Email,
                        PhoneNumber = entity.PhoneNumber,
                        WorkerType = Enum.GetName(typeof(WorkerType), entity.WorkerType.Value),
                        Role = roles,
                        RoomID = entity.RoomID
                    };
                    return Json(output);
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
        /**
         * @api {post} /Auth/Logout Logout
         * @apiVersion 0.1.0
         * @apiName Logout
         * @apiGroup Auth
         *
         *@apiSuccess {String} status User succesfully logged out
         *@apiSuccessExample Success-Response:
         * HTTP/1.1 200 OK
          *     {
          *     "status":"logout"
          *     }
         */
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Json(new { status = "logout" });
        }
        /**
         * @api {post} /Auth/CheckOut CheckOut
         * @apiVersion 0.1.0
         * @apiName CheckOut
         * @apiGroup Auth
         *
         *@apiParam {String} emailOrLogin Email or login of user to be checked out
         *@apiParam {String} roomNumber For verification - number of room that is occupied by requested user
         * 
         *@apiSuccess {String} status User succesfully checked out
         *@apiSuccessExample Success-Response:
         * HTTP/1.1 200 OK
          *     {
          *     "status":"checkedOut"
          *     }
          *@apiError InvalidRole Only user with role Customer can occupy room
          * @apiErrorExample Error-Response:
          * HTTP/1.1 200 OK
          * { 
          *     "status":"invalidRole"
          * }
          * @apiError UserAlreadyCheckedOut If user does not occupy specified room, validation fails
          *  @apiErrorExample Error-Response:
          *  HTTP/1.1 200 OK
          *  {
          *     "status":"userAlreadyCheckedOut"
          *  }
          *  
          * @apiError RoomNotOccupied If specified room is not occupied, validation fails
          *  @apiErrorExample Error-Response:
          *  HTTP/1.1 200 OK
          *  {
          *     "status":"roomNotOccupied"
          *  }
          * 
          * @apiError InvalidInput If Room or User cannot be found
          *  @apiErrorExample Error-Response:
          *  HTTP/1.1 200 OK
          *  { 
          *     "status":"invalidInput"
          *  }
         */
        [HttpPost]
        public async Task<IActionResult> CheckOut([FromBody] string emailOrLogin, string roomNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Room room = db.Rooms.First(q => q.Number == roomNumber);
                    User guestAccount = idb.Users.First(p => p.Email == emailOrLogin || p.UserName == emailOrLogin);

                    if (!(await userManager.IsInRoleAsync(guestAccount, "Customer")))
                    {
                        return Json(new { status = "invalidRole" });
                    }

                    if (guestAccount.Room == null)
                    {
                        return Json(new { status = "userAlreadyCheckedOut" });
                    }

                    if (!room.Occupied)
                    {
                        return Json(new { status = "roomNotOccupied" });
                    }

                    guestAccount.Room = null;
                    guestAccount.RoomID = null;
                    room.GuestFirstName = string.Empty;
                    room.GuestLastName = string.Empty;
                    room.Occupied = false;

                    db.Rooms.Attach(room);
                    db.Entry(room).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    idb.Users.Attach(guestAccount);
                    idb.Entry(guestAccount).State = EntityState.Modified;

                    await idb.SaveChangesAsync();

                    return Json(new { status = "checkedOut" });
                }
                return Json(new { status = "invalidInput" });

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        /**
      * @api {post} /Auth/CheckIn CheckIn
      * @apiVersion 0.1.0
      * @apiName CheckIn
      * @apiGroup Auth
      *
      *@apiParam {String} emailOrLogin Email or login of user to be checked in
      *@apiParam {String} roomNumber Number of room to which user will be checked in
      * 
      *@apiSuccess {String} status User succesfully checked in
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       *     {
       *     "status":"checkedIn"
       *     }
       *@apiError InvalidRole Only user with role Customer can occupy room
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * { 
       *     "status":"invalidRole"
       * }
       * @apiError UserAlreadyCheckedIn One customer can occupy only one room at the same time
       *  @apiErrorExample Error-Response:
       *  HTTP/1.1 200 OK
       *  {
       *     "status":"userAlreadyCheckedIn"
       *  }
       *  
       * @apiError RoomAlreadyOccupied If specified room is already occupied, validation fails
       *  @apiErrorExample Error-Response:
       *  HTTP/1.1 200 OK
       *  {
       *     "status":"roomAlreadyOccupied"
       *  }
       * 
       * @apiError InvalidInput If Room or User cannot be found
       *  @apiErrorExample Error-Response:
       *  HTTP/1.1 200 OK
       *  { 
       *     "status":"invalidInput"
       *  }
      */
        [HttpPost]
        public async Task<IActionResult> CheckIn([FromBody] string emailOrLogin, string roomNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Room room = db.Rooms.First(q => q.Number == roomNumber);
                    User guestAccount = idb.Users.First(p => p.Email == emailOrLogin || p.UserName == emailOrLogin);

                    if (!(await userManager.IsInRoleAsync(guestAccount, "Customer")))  //Only hotel customer can be checked in
                    {
                        return Json(new { status = "invalidRole" });
                    }

                    if (guestAccount.Room != null)
                    {
                        return Json(new { status = "userAlreadyCheckedIn" });
                    }

                    if (room.Occupied)
                    {
                        return Json(new { status = "roomAlreadyOccupied" });
                    }

                    guestAccount.RoomID = room.RoomID;
                    guestAccount.Room = room;
                    room.GuestFirstName = guestAccount.FirstName;
                    room.GuestLastName = guestAccount.LastName;
                    room.Occupied = true;

                    db.Rooms.Attach(room);
                    db.Entry(room).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    idb.Users.Attach(guestAccount);
                    idb.Entry(guestAccount).State = EntityState.Modified;

                    await idb.SaveChangesAsync();

                    return Json(new { status = "checkedIn" });
                }
                return Json(new { status = "invalidInput" });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        /**
      * @api {post} /Auth/Register Register
      * @apiVersion 0.1.0
      * @apiName Register
      * @apiGroup Auth
      *
      *@apiParam {String} Login Login of user
      * @apiParam {String} Password Password of user
      * @apiParam {String} FirstName Optional user name
      * @apiParam {String} LastName Optional user surname
      * @apiParam {String} PhoneNumber Optional contact number
      * @apiParam {String} RoleName One of roles (Worker,Customer,Manager,Administrator)
      * @apiParam {String} Email Required user identifier
      * @apiParam {String} WorkerType One of available types (Cleaner,Technician,None)
      * 
      *@apiSuccess {String} status User succesfully created
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       *     {
       *     "status":"registered"
       *     }
       *@apiError InvalidInput One of entries is not valid
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * { 
       *     "status":"failure"
       * }
       * 
       * @apiError EmailNotUnique Email address have to be unique
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * { 
       *    "status":"userIdentifierNotUnique"
       *    }
       *@apiError RoleNameInvalid Role Name must be valid with existing roles
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * {
       *    "status":"roleNameInvalid"
       *    }
      */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = registerModel.Login,
                    PhoneNumber = registerModel.PhoneNumber,
                    LastName = registerModel.LastName,
                    FirstName = registerModel.FirstName,
                    Email = registerModel.Email,
                    NormalizedEmail = registerModel.Email,
                    WorkerType = (WorkerType)System.Enum.Parse(typeof(WorkerType), registerModel.WorkerType, true)
                };
                var result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, registerModel.RoleName);
                    if (roleResult.Succeeded)
                    {
                        return Json(new { status = "registered" });
                    }
                    else
                    {
                        return Json(new { status = "roleNameInvalid" });
                    }
                }
                else
                {
                    return Json(new { status = "userIdentifierNotUnique" });
                }
            }
            return Json(new { status = "failure" });
        }
    }
}