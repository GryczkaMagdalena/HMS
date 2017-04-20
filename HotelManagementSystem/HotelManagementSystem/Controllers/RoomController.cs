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
    [Authorize]
    [Produces("application/json")]
    [Route("api/Room")]
    public class RoomController : Controller
    {
        private StorageContext storage = new StorageContext();
        
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

        //GET /api/Room/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(Guid id)
        {
            Room room = null;
            try
            {
                room = await storage.Rooms.FindAsync(id);
            }catch(Exception ex)
            {
                return Json(ex);
            }
            return Json(room);
        }


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

        // PUT api/Room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    room.RoomID = id;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (Guid id)
        {
            try
            {
                Room toDelete = await storage.Rooms.FindAsync(id);
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