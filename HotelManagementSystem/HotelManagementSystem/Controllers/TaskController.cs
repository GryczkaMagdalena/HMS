using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Cors;
using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Task/[action]")]
    public class TaskController : Controller
    {
        private readonly IdentityContext _context;
        private readonly UserService userService;
        private readonly TaskDisposer _taskDisposer;
        public TaskController(IdentityContext context,UserManager<User> manager,
            RoleManager<IdentityRole> roles,IPasswordHasher<User> hash ,SignInManager<User>signInManager)
        {
            _context = context;
            userService = new UserService(context, manager, signInManager, hash, roles);
            _taskDisposer = new TaskDisposer(userService);
        }

        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> Task()
        {
            List<Models.Entities.Storage.Task> tasks = await _context.Tasks.ToListAsync();

            var tasksObjectified = tasks.Select(q => new
            {
                TaskID = q.TaskID,
                Describe = q.Describe,
                Room = q.Room,
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
                rule = await _context.Tasks.FindAsync(id);
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
                    _context.Tasks.Attach(value);
                    _context.Entry(value).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Task([FromBody] TaskViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.FirstAsync(q => q.Email == value.Email);
                    var room = await _context.Rooms.FirstAsync(q => q.Number == value.RoomNumber);

                    if (user == null) return NotFound(new { status = "userNotFound" });
                    if (room == null) return NotFound(new { status = "roomNotFound" });
                    //TODO implement with passing actual case when it will be available
                    var Case = new Case()
                    {
                        CaseID=Guid.NewGuid(),Description="desc1",Title="title1",WorkerType=WorkerType.Cleaner
                    };
                    var receiver = await _taskDisposer.FindWorker(Case);
                    var listener = await _taskDisposer.AttachListeningManager(Case, receiver);

                    if (receiver == null) return BadRequest(new {status="All workers busy. Try again later" });

                    var newTask = new Models.Entities.Storage.Task()
                    {
                        TaskID=Guid.NewGuid(),
                        Describe=value.Describe,
                        Room=room,
                        Issuer=user,
                        Receiver= receiver,
                        Listener = listener
                    };


                    await _context.Tasks.AddAsync(newTask);
                    await _context.SaveChangesAsync();
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
                Models.Entities.Storage.Task toDelete = await _context.Tasks.FindAsync(id);
                if (toDelete != null)
                {
                    _context.Tasks.Attach(toDelete);
                    _context.Entry(toDelete).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
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