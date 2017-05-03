using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Cors;

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Task/[action]")]
    public class TaskController : Controller
    {
        private readonly StorageContext _context;

        public TaskController(StorageContext context)
        {
            _context = context;
        }

        private StorageContext storage = new StorageContext();
        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> Task()
        {
            List<Models.Entities.Storage.Task> tasks = await storage.Tasks.ToListAsync();

            var tasksObjectified = tasks.Select(q => new
            {
                TaskID = q.TaskID,
                Describe = q.Describe,
                RoomID = q.RoomID,
                Room = q.Room
            });
            return Json(tasksObjectified);
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] Guid id)
        {
            Models.Entities.Storage.Task rule = null;
            try
            {
                rule = await storage.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(rule);
        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Task([FromRoute] Guid id, [FromBody] Models.Entities.Storage.Task value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.TaskID = id;
                    storage.Tasks.Attach(value);
                    storage.Entry(value).State = EntityState.Modified;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "updated" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // POST: api/Task
        [HttpPost]
        public async Task<IActionResult> Task([FromBody] Models.Entities.Storage.Task value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.TaskID = Guid.NewGuid();
                    await storage.Tasks.AddAsync(value);
                    await storage.SaveChangesAsync();
                    return Json(new { status = "created" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            try
            {
                Models.Entities.Storage.Task toDelete = await storage.Tasks.FindAsync(id);
                if (toDelete != null)
                {
                    storage.Tasks.Attach(toDelete);
                    storage.Entry(toDelete).State = EntityState.Deleted;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "removed" });
                }
                else
                {
                    throw new Exception("Entity not present in database");
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.TaskID == id);
        }
    }
}