using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Task")]
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
            _taskDisposer = new TaskDisposer(userService,_context);
        }

        /**
       * @api {get} /Task List
       * @apiVersion 0.1.2
       * @apiName List
       * @apiGroup Task
       *
       *@apiSuccess {Array} tasks List of all existing tasks
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *   [
        *       {
        *       "TaskID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Describe":"Describtion of task",
        *       "RoomID":"5ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Room":"Connected room"
        *       }
        *   ]
       */
        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Models.Entities.Storage.Task> tasks = await _context.Tasks.Include(q=>q.Case)
                .Include(p=>p.Issuer).Include(q=>q.Listener).Include(q=>q.Receiver).ToListAsync();


            return Json(tasks.Select(q => new
            {
                TaskID = q.TaskID,
                Describe = q.Describe,
                Room = q.Room,
                Issuer = q.Issuer,
                Receiver = q.Receiver,
                Listener = q.Listener,
                Case = q.Case,
            }));
        }
             /**
       * @api {get} /Task/TaskID Read
       * @apiVersion 0.1.2
       * @apiName Read
       * @apiGroup Task
       *
       * @apiParam {GUID} TaskID Task identifier
       * 
       * 
       *@apiSuccess {String} TaskID Task identifier
       * @apiSuccess {String} Description of task
       * @apiSuccess {String} RoomID Room identifier
       * @apiSuccess {Room} Room
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "TaskID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Describe":"Describtion of task",
        *       "RoomID":"5ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Room":"Connected room"
        *       }
        *@apiError NotFound Given ID does not appeal to any of tasks
        *@apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
        * {
        *   "status":"notFound"
        * }
       */

        // GET: api/Task/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read([FromRoute] Guid id)
        {
            Models.Entities.Storage.Task rule = null;
            try
            {
                rule = await _context.Tasks.FindAsync(id);
            }
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
            return Json(rule);
        }
        /**
        * @api {put} /task?TaskID Update
        * @apiVersion 0.1.2
        * @apiName Update
        * @apiGroup Task
        *
        * 
        * 
        *@apiSuccess {String} status task was updated 
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
         * @apiError NotFound task with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 200 OK
         * {
         *  "status":"notFound"
         * }
    */

        // PUT: api/Task/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Models.Entities.Storage.Task value)
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
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
        }
        /**
         * @api {post} /Task Create
         * @apiVersion 0.1.2
         * @apiName Create
         * @apiGroup Task
         *
       * @apiSuccess {String} Email User Email
       * @apiSuccess {String} Numer Numer of room
       * @apiSuccess {String} Title title of case
       * 
         *@apiSuccess {String} status task was created 
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
                    var case_in_task = await _context.Cases.FirstAsync(q => q.Title == value.Title);

                    if (user == null) return NotFound(new { status = "userNotFound" });
                    if (room == null) return NotFound(new { status = "roomNotFound" });

                    var receiver = await _taskDisposer.FindWorker(case_in_task);
                    var listener = await _taskDisposer.AttachListeningManager(case_in_task, receiver);

                    if (receiver == null) return BadRequest(new {status="All workers busy. Try again later" });

                    var newTask = new Models.Entities.Storage.Task()
                    {
                        TaskID=Guid.NewGuid(),
                        Describe=value.Describe,
                        Room=room,
                        Issuer=user,
                        Receiver= receiver,
                        Listener = listener,
                        Case = case_in_task,
                    };

                    receiver.ReceivedTasks.Add(newTask);
                    user.IssuedTasks.Add(newTask);

                    _context.Entry(newTask).State = EntityState.Added;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.Entry(receiver).State = EntityState.Modified;


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
                Console.WriteLine(ex.ToString());
                return Json(new {status="notFound" });
            }
        }

        /**
       * @api {delete} /Task/TaskID Delete
       * @apiVersion 0.1.2
       * @apiName Delete
       * @apiGroup Task
       *
       * @apiParam {GUID} TaskID Task identifier
       * 
       * 
       *@apiSuccess {String} status Task was deleted
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "status":"removed"
        *       }
        * 
        * @apiError NotFound Task with specified ID was not found
        * @apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
        * {
        *  "status":"notFound"
        * }
   */
        // DELETE: api/Task/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
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
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
        }


        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.TaskID == id);
        }
    }
}