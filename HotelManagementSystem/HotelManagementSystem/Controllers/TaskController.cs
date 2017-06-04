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
using Microsoft.Extensions.Logging;
using HotelManagementSystem.Models.Infrastructure.IdentityBase;
using HotelManagementSystem.Models.Helpers;
using HotelManagementSystem.Models.Abstract;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Task")]
    public class TaskController : Controller
    {
        private readonly ILogger _logger;
        private readonly IdentityContext _context;
        private readonly IUserService _userService;
        private readonly TaskDisposer _taskDisposer;
        public TaskController(IdentityContext context,
            IUserService userService, ILogger<TaskController> logger)
        {
            _context = context;
            _userService = userService;
            _taskDisposer = new TaskDisposer(_userService, _context);
            _logger = logger;
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
        *       "taskID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "describe":"Describtion of task",
        *       "roomID":"5ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "room":"Connected room"
        *       }
        *   ]
       */
        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Models.Entities.Storage.Task> tasks = await _context.LazyLoadTasks();

            return Ok(tasks.Select(q => new
            {
                TaskID = q.TaskID,
                Describe = q.Describe,
                Room = q.Room,
                Issuer = q.Issuer.ToJson(),
                Receiver = q.Receiver.ToJson(),
                Listener = q.Listener.ToJson(),
                Case = q.Case,
                TimeOfCreation = q.TimeOfCreation,
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
  *@apiSuccess {String} taskID Task identifier
  * @apiSuccess {String} description of task
  * @apiSuccess {String} roomID Room identifier
  * @apiSuccess {Room} room
  *@apiSuccessExample Success-Response:
  * HTTP/1.1 200 OK
   *       {
   *       "taskID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
   *       "describe":"Describtion of task",
   *       "roomID":"5ba83f3c-4ea4-4da4-9c06-e986a8273800",
   *       "room":"Connected room"
   *       }
   *@apiError NotFound Given ID does not appeal to any of tasks
   *@apiErrorExample Error-Response:
   * HTTP/1.1 404 NotFound
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
                rule = await _context.LazyLoadTask(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
            return Ok(new
            {
                TaskID = rule.TaskID,
                Describe = rule.Describe,
                Room = rule.Room,
                Issuer = rule.Issuer.ToJson(),
                Receiver = rule.Receiver.ToJson(),
                Listener = rule.Listener.ToJson(),
                Case = rule.Case,
                TimeOfCreation = rule.TimeOfCreation
            });
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
         * HTTP/1.1 400 BadRequest
         * {
         *   "status":"failure"
         * }
         * 
         * @apiError NotFound task with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 NotFound
         * {
         *  "status":"notFound"
         * }
    */

        // PUT: api/Task/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskViewModel value)
        {
            if (ModelState.IsValid)
            {
                using (var safeTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = await _context.LazyLoadUserByEmail(value.Email) as Customer;
                        var room = await _context.Rooms.Include(q => q.User)
                            .FirstAsync(q => q.Number == value.RoomNumber);
                        var case_in_task = await _context.Cases.FirstAsync(q => q.Title == value.Title);

                        if (user == null) return NotFound(new { status = "userNotFound" });
                        if (room == null) return NotFound(new { status = "roomNotFound" });

                        var receiver = await _taskDisposer.FindWorker(case_in_task);
                        var listener = await _taskDisposer.AttachListeningManager(case_in_task, receiver);

                        if (receiver == null) return BadRequest(new { status = "All workers busy. Try again later" });

                        var taskToUpdate = await _context.LazyLoadTask(id);

                        taskToUpdate.Listener.ListenedTasks.Remove(taskToUpdate);
                        taskToUpdate.Receiver.ReceivedTasks.Remove(taskToUpdate);
                        taskToUpdate.Issuer.IssuedTasks.Remove(taskToUpdate);

                        taskToUpdate = new Models.Entities.Storage.Task()
                        {
                            TaskID = id,
                            Describe = value.Describe,
                            Room = room,
                            Issuer = user,
                            Receiver = receiver,
                            Listener = listener,
                            Case = case_in_task,
                            TimeOfCreation=DateTime.Now
                        };

                        receiver.ReceivedTasks.Add(taskToUpdate);
                        user.IssuedTasks.Add(taskToUpdate);

                        _context.Entry(taskToUpdate).State = EntityState.Modified;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.Entry(receiver).State = EntityState.Modified;


                        await _context.SaveChangesAsync();
                        safeTransaction.Commit();
                        return Ok(new { status = "created" });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        safeTransaction.Rollback();
                        return BadRequest(new { status = "failure" });
                    }
                }
            }
            else
            {
                return BadRequest(new { status = "failure" });
            }
        }
        /**
         * @api {post} /Task Create
         * @apiVersion 0.1.2
         * @apiName Create
         * @apiGroup Task
         *
       * @apiSuccess {String} email User Email
       * @apiSuccess {String} numer Numer of room
       * @apiSuccess {String} title title of case
       * 
       * @apiParam {String} Title Title of case to be attached
       * @apiParam {String} Email Email of issuer
       * @apiParam {String} Describe Description of task (to be visualised for manager for ex.), can contain Description from Case and Number of Room
       * @apiParam {String} RoomNumber Number of room from which task is issued
       * 
         *@apiSuccess {String} status task was created 
         *@apiSuccessExample Success-Response:
         * HTTP/1.1 200 OK
          *       {
          *       "status":"created"
          *       }
          *@apiError InvalidInput One of inputs was null or invalid
          *@apiErrorExample Error-Response:
          * HTTP/1.1 400 BadRequest
          * {
          *   "status":"failure"
          * }
          * 
          * @apiError BusyWorkers All workers are busy or too many not in work to take task
          *@apiErrorExample Error-Response:
          * HTTP/1.1 400 BadRequest
          * {
          *   "status":"All workers busy. Try again later"
          * }
          * 
          * @apiError NotFound User with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 NotFound
         * {
         *  "status":"userNotFound"
         * }
         *  @apiError NotFound Room with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 NotFound
         * {
         *  "status":"roomNotFound"
         * }
     */

        // POST: api/Task
        [HttpPost]
        public async Task<IActionResult> Task([FromBody] TaskViewModel value)
        {
            if (ModelState.IsValid)
            {
                using (var safeTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = await _context.LazyLoadUserByEmail(value.Email) as Customer;
                        var room = await _context.Rooms.Include(q => q.User).FirstAsync(q => q.Number == value.RoomNumber);
                        var case_in_task = await _context.Cases.FirstAsync(q => q.Title == value.Title);

                        if (user == null) return NotFound(new { status = "userNotFound" });
                        if (room == null) return NotFound(new { status = "roomNotFound" });

                        var receiver = await _taskDisposer.FindWorker(case_in_task);
                        var listener = await _taskDisposer.AttachListeningManager(case_in_task, receiver);

                        if (receiver == null) return BadRequest(new { status = "All workers busy. Try again later" });

                        var newTask = new Models.Entities.Storage.Task()
                        {
                            TaskID = Guid.NewGuid(),
                            Describe = value.Describe,
                            Room = room,
                            Issuer = user,
                            Receiver = receiver,
                            Listener = listener,
                            Case = case_in_task,
                            TimeOfCreation = DateTime.Now,
                        };

                        receiver.ReceivedTasks.Add(newTask);
                        user.IssuedTasks.Add(newTask);

                        _context.Entry(newTask).State = EntityState.Added;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.Entry(receiver).State = EntityState.Modified;


                        await _context.SaveChangesAsync();
                        safeTransaction.Commit();
                        return Ok(new { status = "created" });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        safeTransaction.Rollback();
                        return BadRequest(new { status = "failure" });
                    }
                }
            }
            else
            {
                return BadRequest(new { status = "failure" });
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
        * HTTP/1.1 404 NotFound
        * {
        *  "status":"notFound"
        * }
        * @apiError InvalidInput One of inputs was null or invalid
          *@apiErrorExample Error-Response:
          * HTTP/1.1 400 BadRequest
          * {
          *   "status":"failure"
          * }
   */
        // DELETE: api/Task/{id}
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Models.Entities.Storage.Task toDelete = await _context.LazyLoadTask(id);
                if (toDelete != null)
                {
                    _context.Tasks.Attach(toDelete);
                    _context.Entry(toDelete).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "removed" });
                }
                else
                {
                    return NotFound(new { status = "notFound" });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { status = "failure" });
            }
        }

        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.TaskID == id);
        }


    }
}
