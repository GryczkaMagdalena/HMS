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

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/Room")]
    public class RoomController : Controller
    {
        private StorageContext storage = new StorageContext();
        
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
       *    "RoomID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
       *    "GuestFirstName":"Marco",
       *    "GuestLastName":"Polo",
       *    "Number":9,
       *    "Occupied":false
       *    }
       * ]
       * 
      */
        // GET api/Room
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Room> rooms = await storage.Rooms.ToListAsync();
            var roomsObjectified = rooms.Select(q => new
            {
                RoomID = q.RoomID,
                GuestFirstName = q.GuestFirstName,
                GuestLastName = q.GuestLastName,
                Number = q.Number,
                Occupied = q.Occupied
            });
            return Json(roomsObjectified);
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
   * @apiSuccess {String} RoomID Room identifier
   * @apiSuccess {String} GuestFirstName If room is occupied here will be name of client
   * @apiSuccess {String} GuestLastName If room is occupied here will be surname of client
   * @apiSuccess {Boolean} Occupied Is room free
   * @apiSuccess {Number} Number Number of room
   * 
   *@apiSuccessExample Success-Response:
   * HTTP/1.1 200 OK
    *      { 
   *    "RoomID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
   *    "GuestFirstName":"Marco",
   *    "GuestLastName":"Polo",
   *    "Number":9,
   *    "Occupied":false
   *    }
    *@apiError NotFound Given ID does not appeal to any of rooms
    *@apiErrorExample Error-Response:
    * HTTP/1.1 200 OK
    * {
    *   "status":"notFound"
    * }
   */
        //GET /api/Room/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(Guid RoomID)
        {
            Room room = null;
            try
            {
                room = await storage.Rooms.FindAsync(RoomID);
            }catch(Exception)
            {
                return Json(new { status = "notFound" });
            }
            return Json(room);
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
                    await storage.Rooms.AddAsync(room);
                    await storage.SaveChangesAsync();
                    return Json(new { status = "created" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
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
       * HTTP/1.1 200 OK
       * {
       *   "status":"failure"
       * }
       * 
       * @apiError NotFound Room with specified ID was not found
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * {
       *  "status":"notFound"
       * }
  */
        // PUT api/Room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid RoomID, [FromBody] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var origin = await storage.Rooms.FindAsync(RoomID);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = room;
                    origin.RoomID = RoomID;
                    storage.Rooms.Attach(room);
                    storage.Entry(room).State = EntityState.Modified;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "updated" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
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
       * HTTP/1.1 200 OK
       * {
       *  "status":"notFound"
       * }
  */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (Guid RoomID)
        {
            try
            {
                Room toDelete = await storage.Rooms.FindAsync(RoomID);
                if (toDelete != null)
                {
                    storage.Rooms.Attach(toDelete);
                    storage.Entry(toDelete).State = EntityState.Deleted;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "removed" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
            }
        }
    }
}
