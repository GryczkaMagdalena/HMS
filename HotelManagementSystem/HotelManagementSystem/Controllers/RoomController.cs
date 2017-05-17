using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Room")]
    public class RoomController : Controller
    {
        private IdentityContext _context;
        private ILogger _logger;
        private UserService _userService;
        public RoomController(ILogger<RoomController> logger, IdentityContext context,
            UserManager<User> userManager,IPasswordHasher<User> hasher,RoleManager<IdentityRole> roleManager,SignInManager<User> signInManager)
        {
            _context = context;
            _logger = logger;
            _userService = new UserService(context, userManager, signInManager, hasher, roleManager);
        }

        /**
    * @api {get} /Room List
    * @apiVersion 0.1.0
    * @apiName List
    * @apiGroup Room
    *
    *@apiSuccess {Array} rules List of all rooms
    * 
    *@apiSuccessExample Success-Response:
    * HTTP/1.1 200 OK
     * [
     *    { 
     *    "roomID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
     *    "user":{
     *      "userID":"4ba83f3c-4ea4-4da4-9c06-e986a827230",
     *      "lastName":"Franz",
     *      "firstName":"Artur",
     *    },
     *    "number":9,
     *    "occupied":false
     *    }
     * ]
     * 
    */
        // GET api/Room
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Room> rooms = await _context.Rooms.ToListAsync();
            if (rooms.Any(q => q.User == null))
            {
                var roomsObjectified = rooms.Select(q => new
                {
                    RoomID = q.RoomID,
                    User = q,
                    Number = q.Number,
                    Occupied = q.Occupied
                });
                return Ok(roomsObjectified);
            }
            else
            {
                var roomsObjectified = rooms.Select(q => new
                {
                    RoomID = q.RoomID,
                    User = new
                    {
                        UserID = q.UserID,
                        FirstName = q.User.FirstName,
                        LastName = q.User.LastName
                    },
                    Number = q.Number,
                    Occupied = q.Occupied
                });
                return Ok(roomsObjectified);
            }
            
        }
        /**
   * @api {get} /Room?RoomID Read
   * @apiVersion 0.1.0
   * @apiName Read
   * @apiGroup Room
   *
   * @apiParam {GUID} RoomID Room identifier
   * 
   * 
   * @apiSuccess {String} roomID Room identifier
   * @apiSuccess {GUID} userID If room is occupied here will be id of client
   * @apiSuccess {String} firstName If room is occupied - first name of client
   * @apiSuccess {String} lastName If room is occupied - last name of client
   * @apiSuccess {Boolean} Occupied Is room free
   * @apiSuccess {Number} Number Number of room
   * 
   *@apiSuccessExample Success-Response:
   * HTTP/1.1 200 OK
    *   { 
     *    "roomID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
     *    "user":{
     *      "userID":"4ba83f3c-4ea4-4da4-9c06-e986a827230",
     *      "lastName":"Franz",
     *      "firstName":"Artur",
     *    },
     *    "number":9,
     *    "occupied":false
     *    }
    *@apiError NotFound Given ID does not appeal to any of rooms
    *@apiErrorExample Error-Response:
    * HTTP/1.1 404 NotFound
    * {
    *   "status":"notFound"
    * }
   */
        //GET /api/Room/{id}
        [HttpGet("{RoomID}")]
        public async Task<IActionResult> Read(Guid RoomID)
        {
            Room room = null;
            try
            {
                room = await _context.Rooms.FindAsync(RoomID);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
            if (room.User == null)
            {
                return Ok(new
                {
                    RoomID = room.RoomID,
                    User = room.User,
                    Number = room.Number,
                    Occupied = room.Occupied
                });
            }
            else
            {
                return Ok(new
                {
                    RoomID = room.RoomID,
                    User = new
                    {
                        UserID = room.UserID,
                        FirstName = room.User.FirstName,
                        LastName = room.User.LastName
                    },
                    Number = room.Number,
                    Occupied = room.Occupied
                });
            }
        }

        /**
       * @api {post} /Room Create
       * @apiVersion 0.1.0
       * @apiName Create
       * @apiGroup Room
       *
       * @apiParam {Number} Number Number of room
       * 
       * 
       *@apiSuccess {String} status Room was created 
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "status":"created"
        *       }
        *@apiError InvalidInput One of inputs was null or invalid
        *@apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
        * {
        *   "status":"failure"
        * }
        * @apiError NotFound Referenced ID was not found
        *@apiErrorExample Error-Response:
        * HTTP/1.1 404 NotFound
        * {
        *   "status":"notFound"
        * }
   */
        // POST api/Room
        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    room.RoomID = Guid.NewGuid();
                    await _context.Rooms.AddAsync(room);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "created" });
                }
                else
                {
                    return BadRequest(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
        }
        /**
      * @api {put} /Room?RoomID Update
      * @apiVersion 0.1.0
      * @apiName Update
      * @apiGroup Room
      *
      * @apiParam {GUID} RoomID Room identifier
      * @apiParam {Number} Number Room number
      * 
      *@apiSuccess {String} status Room was updated 
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       *       {
       *       "status":"updated"
       *       }
       *@apiError InvalidInput One of inputs was null or invalid
       *@apiErrorExample Error-Response:
       * HTTP/1.1 400 BadRequest
       * {
       *   "status":"failure"
       * }
       * 
       * @apiError NotFound Room with specified ID was not found
       * @apiErrorExample Error-Response:
       * HTTP/1.1 404 NotFound
       * {
       *  "status":"notFound"
       * }
  */
        // PUT api/Room/{id}
        [HttpPut("{RoomID}")]
        public async Task<IActionResult> Update([FromRoute] Guid RoomID, [FromBody] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var origin = await _context.Rooms.FindAsync(RoomID);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = room;
                    origin.RoomID = RoomID;
                    _context.Rooms.Attach(room);
                    _context.Entry(room).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "updated" });
                }
                else
                {
                    return BadRequest(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
        }

        // DELETE api/Room/{id}
        /**
        * @api {delete} /Room?RoomID Delete
        * @apiVersion 0.1.0
        * @apiName Delete
        * @apiGroup Room
        *
        * @apiParam {GUID} RoomID Room identifier
        * 
        * 
        *@apiSuccess {String} status Room was deleted
        *@apiSuccessExample Success-Response:
        * HTTP/1.1 200 OK
         *       {
         *       "status":"removed"
         *       }
         * 
         * @apiError NotFound Room with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 NotFound
         * {
         *  "status":"notFound"
         * }
         * @apiError InvalidData Reference or object was not correct
         *@apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *   "status":"failure"
         * }
    */
        [HttpDelete("{RoomID}")]
        public async Task<IActionResult> Delete (Guid RoomID)
        {
            try
            {
                Room toDelete = await _context.Rooms.FindAsync(RoomID);
                if (toDelete != null)
                {
                    _context.Rooms.Attach(toDelete);
                    _context.Entry(toDelete).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "removed" });
                }
                else
                {
                    return NotFound(new { status = "notFound" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { status = "failure" });
            }
        }

        /**
        * @api {get} /Room/RoomNumber GetRoomNumber
        * @apiVersion 0.1.0
        * @apiName GetRoomNumber
        * @apiGroup Room
        * 
        *@apiSuccess {String} roomNumber Number of room
        *@apiSuccessExample Success-Response:
        * HTTP/1.1 200 OK
        * {
        *   "roomNumber":"1"
        * }
        * 
        * @apiError BadRole Only user with role Customer can access this method
        * @apiErrorExample Error-Response:
        * HTTP/1.1 403 Unauthorized
        * 
        * @apiError UserNotCheckedIn Current user is not checked in any room
        * @apiErrorExample Error-Response:
        * HTTP/1.1 400 BadRequest
        * {
        *   "status":"userNotCheckedIn"
        * }
        * 
        */ 
        [Authorize(Roles ="Customer")]
        [HttpGet("RoomNumber")]
        public async Task<IActionResult> RoomNumber()
        {
            var login = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUsername(login);
                if (user.Room != null)
                {
                    return Ok(new { roomNumber = user.Room.Number });
                }
                else
                {
                    return NotFound(new { status = "userNotCheckedIn" });
                }
        }
    }
}
