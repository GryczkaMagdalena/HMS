using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Owin;
namespace HotelManagementSystem.Controllers
{

    [EnableCors("HotelCorsPolicy")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Auth/[action]")]
    public class AuthController : Controller
    {
        private UserService _userService;
        private IdentityContext _context;
        private readonly ILogger _logger;

        public AuthController(
            SignInManager<User> signInManager,
            IPasswordHasher<User> hash,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthController> logger,
            ApplicationUserManager userManager,
            IdentityContext context
            )
        {
            _userService = new UserService(userManager, signInManager, hash, roleManager);
            _logger = logger;
            _context = context;
        }
        /**
            * @apiDeprecated do not use it.  
            * @api {get} /Auth/Login Login - clear cookie
            * @apiVersion 0.1.1
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
        public IActionResult Login()
        {
            return Json(new { status = "clear" });
        }

        /**
          * @apiDeprecated use now (#Auth:Token). 
          * @api {post} /Auth/Login Login
          * @apiVersion 0.1.1
          * @apiName PostLogin
          * @apiGroup Auth
          *
          * @apiParam {String} Login Email or login of user
          * @apiParam {String} Password User's password
          *
          * @apiSuccess {String} firstName First name of user.
          * @apiSuccess {String} lastName Last name of user.
          * @apiSuccess {String} email Optional email address of user
          * @apiSuccess {String} phoneNumber Optional phone number of user
          * @apiSuccess {String} workerType One of available types (Cleaner,Technician,None).
          * @apiSuccess {Array} role All roles that particular user have 
          * @apiSuccess {GUID} roomID Optional parameter - only guests have this not-null 
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
          * @apiError MissingData Login or Password are missing.
          * @apiErrorExample Error-Response:
          * HTTP/1.1 200 OK
          * {
          *  "status":"fail"
          * }
          * 
          * @apiError Unathorized This User does not exist or password is invalid.
          * @apiErrorExample Error-Response:
          * HTTP/1.1 200 OK
          * {
          *     "status":"unauthorized"
          * }
      */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.PasswordSignInAsync(user, user.Password);
                if (result.Succeeded)
                {
                    var entity = _context.Users.First(q => q.UserName == user.Login);
                    var roles = await _userService.GetUserRoles(entity);

                    var output = new
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Email = entity.Email,
                        PhoneNumber = entity.PhoneNumber,
                        WorkerType = Enum.GetName(typeof(WorkerType), entity.WorkerType.Value),
                        Role = roles,
                        Room = entity.Room
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
            await _userService.SignOut();
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
                    Room room = _context.Rooms.First(q => q.Number == roomNumber);
                    User guestAccount = _context.Users.First(p => p.Email == emailOrLogin || p.UserName == emailOrLogin);

                    if (!(await _userService.IsInRoleAsync(guestAccount, "Customer")))
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
                    guestAccount.Room = null;

                    room.User = null;
                    room.Occupied = false;

                    _context.Rooms.Attach(room);
                    _context.Entry(room).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    _context.Users.Attach(guestAccount);
                    _context.Entry(guestAccount).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

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
            if (ModelState.IsValid)
            {
                using (var safeTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Room room = _context.Rooms.First(q => q.Number == roomNumber);
                        User guestAccount = _context.Users.First(p => p.Email == emailOrLogin || p.UserName == emailOrLogin);

                        if (!(await _userService.IsInRoleAsync(guestAccount, "Customer")))  //Only hotel customer can be checked in
                        {
                            return BadRequest(new { status = "invalidRole" });
                        }

                        if (guestAccount.Room != null)
                        {
                            return BadRequest(new { status = "userAlreadyCheckedIn" });
                        }

                        if (room.Occupied)
                        {
                            return BadRequest(new { status = "roomAlreadyOccupied" });
                        }

                        guestAccount.Room = room;
                        room.User = guestAccount;
                        room.Occupied = true;

                        _context.Rooms.Attach(room);
                        _context.Entry(room).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        _context.Users.Attach(guestAccount);
                        _context.Entry(guestAccount).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(new { status = "checkedIn" });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        return NotFound(new { status = "internalError" });
                    }
                }
            }
            return BadRequest(new { status = "invalidInput" });
        }

        /**
      * @api {post} /Auth/Register Register
      * @apiVersion 0.1.0
      * @apiName Register
      * @apiGroup Auth
      *
      *@apiParam {String} login Login of user
      * @apiParam {String} password Password of user
      * @apiParam {String} firstName Optional user name
      * @apiParam {String} lastName Optional user surname
      * @apiParam {String} phoneNumber Optional contact number
      * @apiParam {String} roleName One of roles (Worker,Customer,Manager,Administrator)
      * @apiParam {String} email Required user identifier
      * @apiParam {String} workerType One of available types (Cleaner,Technician,None)
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
                    ReceivedTasks = new List<Models.Entities.Storage.Task>(),
                    ListenedTasks = new List<Models.Entities.Storage.Task>(),
                    Shifts = new List<Shift>(),
                    IssuedTasks = new List<Models.Entities.Storage.Task>(),
                    NormalizedEmail = registerModel.Email,
                    WorkerType = (WorkerType)System.Enum.Parse(typeof(WorkerType), registerModel.WorkerType, true)
                };
                var result = await _userService.CreateUser(user, registerModel.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userService.AddUserToRole(registerModel.RoleName, user.Id);
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

        /**
         * @api {post} /Auth/Token Token
         * @apiVersion 0.1.1
         * @apiName TokenLogin
         * @apiGroup Auth
         *
         * @apiParam {String} Login Email or login of user
         * @apiParam {String} Password User's password
         *
         * @apiSuccess {String} firstName First name of user.
         * @apiSuccess {String} lastName Last name of user.
         * @apiSuccess {String} email Optional email address of user
         * @apiSuccess {String} workerType One of available types (Cleaner,Technician,None).
         * @apiSuccess {Array} role All roles that particular user have 
         * @apiSuccess {GUID} roomID Optional parameter - only guests have this not-null 
         * @apiSuccess {Token} token Authentication token that should be send in every response as header (headerKey:Authenticate, headerValue: "bearer " + token)
          *@apiSuccess {DateTime} expiration Date when token expires
         * @apiSuccessExample Success-Response:
         *     HTTP/1.1 200 OK
         *     {
         *     "firstName":"Abraham",
         *     "lastName":"Lincoln",
         *     "email":"president@usa.pl",
         *     "workerType":"Technician",
         *     "role":["Worker"]
         *     "token":"blablabla121212",
         *     "expiration":"2017-05-07T20:49:48Z"
         *     }
         * @apiError LoginFailed Password is invalid.
         * @apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *  "status":"failedToLogin"
         * }
         * 
         * @apiError InternalError This User does not exist.
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 BadRequest
         * {
         *     "status":"failed"
         * }
     */

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] UserViewModel userModel)
        {
            try
            {
                var user = await _userService.GetUserByUsername(userModel.Login);
                if (user != null)
                {
                    if (_userService.VerifyHashedPassword(user, userModel.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userService.GetClaims(user);
                        var claims = new[]
                           {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                new Claim(ClaimTypes.Role,await _userService.MainRole(user)),
                                new Claim(ClaimTypes.GroupSid,Enum.GetName(typeof(WorkerType),user.WorkerType))
                            }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HotelowaMuffinka"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.UtcNow.AddHours(8),
                            signingCredentials: creds,
                            issuer: "http://hotelmanagementsystem.azurewebsites.net/",
                            audience: "http://hotelmanagementsystem.azurewebsites.net/"
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user = new
                            {
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                WorkerType = Enum.GetName(typeof(WorkerType), user.WorkerType),
                                Roles = await _userService.GetUserRoles(user),
                                Room = user.Room
                            }
                        });
                    }
                }
            }
            catch (Exception)
            {
                return NotFound(new { status = "failed" });
            }
            return BadRequest(new { status = "failedToLogin" });
        }
        /**
        * @api {post} /Auth/AddToRole AddToRole
        * @apiVersion 0.1.1
        * @apiName AddToRole
        * @apiGroup Auth
        *
        * @apiParam {GUID} UserID ID of user
        * @apiParam {String} roleName Name of role
        * 
        * @apiSuccess {String} status Status of successful response
        * @apiSuccessExample Success-Response:
        *     HTTP/1.1 200 OK
        *     {
        *       "status":"User added to role Customer"
        *     }
        *@apiError RoleNotFound Role name is invalid.
         * @apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *  "status":"Role does not exists"
         * }  
         * @apiError UserError User cannot be added to role.
         * @apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *      "status":User cannot be added to role Customer"
         * }
         * 
         * @apiError UserNotExists User cannot be found.
         * @apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *      "status":User does not exists"
         * }
        */
        [HttpPost("{UserID}")]
        public async Task<IActionResult> AddToRole([FromRoute]Guid UserID, [FromBody] String roleName)
        {
            try
            {
                if (await _userService.RoleExists(roleName))
                {
                    var result = await _userService.AddUserToRole(roleName, UserID.ToString());
                    if (result.Succeeded)
                    {
                        return Ok(new { status = "User added to role " + roleName });
                    }
                    else
                    {
                        return BadRequest(new { status = "User cannot be added to role" + roleName });
                    }
                }
                else
                {
                    return NotFound(new { status = "Role does not exists" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.ToString());
                return NotFound(new { status = "User does not exists" });
            }
        }
        /**
    * @api {post} /Auth/GetUserRoles GetUserID
    * @apiVersion 0.1.3
    * @apiName GetUserID
    * @apiGroup Auth
    * 
    *  @apiSuccess {GUID} id ID of currently logged user
    *  @apiSuccessExample Success-Response
    *   HTTP/1.1 200 OK
     *     {
     *       "id":"some29299guid"
     *     }
*/
        [HttpGet]
        public async Task<IActionResult> GetUserID()
        {
            var login = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUsername(login);
            return Ok(new { id = user.Id });
        }
        private Task<User> GetCurrentUserAsync(ClaimsPrincipal user) => _userService.GetUserAsync(user);
        /**
     * @api {post} /Auth/RemoveFromRole RemoveFromRole
     * @apiVersion 0.1.3
     * @apiName RemoveFromRole
     * @apiGroup Auth
     *
     * @apiParam {GUID} UserID ID of user
     * @apiParam {String} roleName Name of role
     * 
     * @apiSuccess {String} status Status of successful response
     * @apiSuccessExample Success-Response:
     *     HTTP/1.1 200 OK
     *     {
     *       "status":"User removed from role Customer"
     *     }
     *@apiError RoleNotFound Role name is invalid.
      * @apiErrorExample Error-Response:
      * HTTP/1.1 400 BadRequest
      * {
      *  "status":"Role does not exists"
      * }  
      * @apiError UserError User cannot be added to role.
      * @apiErrorExample Error-Response:
      * HTTP/1.1 400 BadRequest
      * {
      *      "status":User cannot be removed from role Customer"
      * }
      * 
      * @apiError UserNotExists User cannot be found.
      * @apiErrorExample Error-Response:
      * HTTP/1.1 400 BadRequest
      * {
      *      "status":User does not exists"
      * }
     */
        [HttpDelete("{UserID}")]
        public async Task<IActionResult> RemoveFromRole([FromRoute]Guid UserID, [FromBody] String roleName)
        {
            try
            {
                if (await _userService.RoleExists(roleName) && await _context.Users.AnyAsync(q => q.Id == UserID.ToString()))
                {
                    var result = await _userService.RemoveFromRole(roleName, UserID.ToString());
                    if (result.Succeeded)
                    {
                        return Ok(new { status = "User removed from role " + roleName });
                    }
                    else
                    {
                        return BadRequest(new { status = "User cannot be removed from role" + roleName });
                    }
                }
                else
                {
                    return NotFound(new { status = "Role does not exists" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.ToString());
                return NotFound(new { status = "User does not exists" });
            }
        }
        /**
      * @api {post} /Auth/GetUserRoles GetUserRoles
      * @apiVersion 0.1.1
      * @apiName GetUserRoles
      * @apiGroup Auth
      *
      * @apiParam {GUID} UserID ID of user
      * 
      *  @apiSuccess {List} roles Roles of user
      * 
      * @apiError UserNotExists User cannot be found.
       * @apiErrorExample Error-Response:
       * HTTP/1.1 400 BadRequest
       * {
       *      "status":User does not exists"
       * }
      */
        [HttpGet("{UserID}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid UserID)
        {
            try
            {
                var user = await _context.Users.FindAsync(UserID.ToString());
                var roles = await _userService.GetUserRoles(user);
                return Ok(new { roles });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.ToString());
                return NotFound(new { status = "User does not exists" });
            }
        }
    }
}