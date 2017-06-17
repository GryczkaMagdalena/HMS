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
using System.Security.Claims;

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
       * @apiVersion 0.1.5
       * @apiName List
       * @apiGroup Task
       *
       *@apiSuccess {Array} tasks List of all existing tasks
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *   [
        *       {
                    "taskID": "c49a6e23-e767-4904-b41a-4c31c3e80ac1",
                    "describe": "Do something for me",
                    "room": {
                      "roomID": "d89fcd07-efba-4ee0-aa10-242c872454d1",
                      "number": "1",
                      "userID": null,
                      "occupied": false
                    },
                    "issuer": {
                      "userID": "377dcd34-bbff-4bdd-afc8-5e760ef1f1fd",
                      "firstName": "Tom",
                      "lastName": "Postman",
                      "email": "guest1@hms.com",
                      "room": null
                    },
                    "receiver": {
                      "userID": "a03df2d1-f001-4848-973a-971033e5bb60",
                      "firstName": "Alfons",
                      "lastName": "Padlina",
                      "email": "worker8@hms.com",
                      "workerType": "Technician"
                    },
                    "listener": null,
                    "case": {
                      "caseID": "cee9dfb3-09f1-4846-aa32-3059ac6279e8",
                      "title": "TestCase",
                      "description": "Do something for me",
                      "workerType": 1,
                      "estimatedTime": "01:00:00"
                    },
                    "timeOfCreation": "2017-06-05T12:15:52.0584134",
                    "status": "Done",
                    "priority": "Emergency"
                  }
        *   ]
       */
        // GET: api/Task
        [HttpGet]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> List()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.LazyLoadWorkerByEmail(email) as Worker;
            List<Models.Entities.Storage.Task> tasks = user.ReceivedTasks.ToList();
            return Ok(tasks.Select(q => new
            {
                TaskID = q.TaskID,
                Describe = q.Describe,
                Room = _context.Rooms.Find(q.RoomID),
                Issuer = q.Issuer.ToJson(),
                Receiver = q.Receiver.ToJson(),
                Listener = q.Listener.ToJson(),
                Case = _context.Cases.Find(q.CaseID),
                TimeOfCreation = q.TimeOfCreation,
                Status = Enum.GetName(typeof(Status), q.Status),
                Priority = Enum.GetName(typeof(Priority), q.Priority)
            }));
        }
        /**
  * @api {get} /Task/TaskID Read
  * @apiVersion 0.1.5
  * @apiName Read
  * @apiGroup Task
  *
  * @apiParam {GUID} TaskID Task identifier
  * 
  * 
  *@apiSuccess {String} taskID Task identifier
  * @apiSuccess {String} describe of task
  * @apiSuccess {String} roomID Room identifier
  * @apiSuccess {Room} room Room entity
  * @apiSuccess {Customer} issuer Customer entity
  * @apiSuccess {Worker} receiver Worker entity
  * @apiSuccess {Manager} listener Manager entity
  * @apiSuccess {Case} case Case entity 
  * @apiSuccess {DateTime} timeOfCreation 
    @apiSuccess {Status} status Status of task
    @apiSuccess {Priority} priority Priority of task
  *@apiSuccessExample Success-Response:
  * HTTP/1.1 200 OK
   *       {
            "taskID": "c49a6e23-e767-4904-b41a-4c31c3e80ac1",
            "describe": "Do something for me",
            "room": {
              "roomID": "d89fcd07-efba-4ee0-aa10-242c872454d1",
              "number": "1",
              "userID": null,
              "occupied": false
            },
            "issuer": {
              "userID": "377dcd34-bbff-4bdd-afc8-5e760ef1f1fd",
              "firstName": "Tom",
              "lastName": "Postman",
              "email": "guest1@hms.com",
              "room": null
            },
            "receiver": {
              "userID": "a03df2d1-f001-4848-973a-971033e5bb60",
              "firstName": "Alfons",
              "lastName": "Padlina",
              "email": "worker8@hms.com",
              "workerType": "Technician"
            },
            "listener": null,
            "case": {
              "caseID": "cee9dfb3-09f1-4846-aa32-3059ac6279e8",
              "title": "TestCase",
              "description": "Do something for me",
              "workerType": 1,
              "estimatedTime": "01:00:00"
            },
            "timeOfCreation": "2017-06-05T12:15:52.0584134",
            "status": "Done",
            "priority": "Emergency"
          }
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
                TimeOfCreation = rule.TimeOfCreation,
                Status = Enum.GetName(typeof(Status), rule.Status),
                Priority = Enum.GetName(typeof(Priority), rule.Priority)
            });
        }
        /**
        * @api {put} /task?TaskID Update
        * @apiVersion 0.1.5
        * @apiName Update
        * @apiGroup Task
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
                        var taskToUpdate = await _context.LazyLoadTask(id);

                        if (receiver == null)
                        {
                            taskToUpdate.Listener?.ListenedTasks.Remove(taskToUpdate);
                            taskToUpdate.Receiver?.ReceivedTasks.Remove(taskToUpdate);
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
                                TimeOfCreation = DateTime.Now,
                                Status = Status.Unassigned,
                                Priority = Priority.Compulsory
                            };

                            user.IssuedTasks.Add(taskToUpdate);

                            _context.Entry(taskToUpdate).State = EntityState.Modified;
                            _context.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
                            taskToUpdate.Listener?.ListenedTasks.Remove(taskToUpdate);
                            taskToUpdate.Receiver?.ReceivedTasks.Remove(taskToUpdate);
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
                                TimeOfCreation = DateTime.Now,
                                Status = Status.Assigned,
                                Priority = Priority.Medium
                            };

                            receiver.ReceivedTasks.Add(taskToUpdate);
                            user.IssuedTasks.Add(taskToUpdate);

                            _context.Entry(taskToUpdate).State = EntityState.Modified;
                            _context.Entry(user).State = EntityState.Modified;
                            _context.Entry(receiver).State = EntityState.Modified;

                        }
                        await _context.SaveChangesAsync();
                        safeTransaction.Commit();
                        return Ok(new { status = "updated" });
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
         * @apiVersion 0.1.5
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

                        if (receiver == null)
                        {
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
                                Status = Status.Unassigned,
                                Priority = Priority.Compulsory
                            };
                            user.IssuedTasks.Add(newTask);

                            _context.Entry(newTask).State = EntityState.Added;
                            _context.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
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
                                Status = Status.Assigned,
                                Priority = Priority.Medium
                            };

                            receiver.ReceivedTasks.Add(newTask);
                            user.IssuedTasks.Add(newTask);
                            if (listener != null)
                            {
                                listener.ListenedTasks.Add(newTask);
                                _context.Entry(listener).State = EntityState.Modified;
                            }
                            _context.Entry(newTask).State = EntityState.Added;
                            _context.Entry(user).State = EntityState.Modified;
                            _context.Entry(receiver).State = EntityState.Modified;
                        }

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
        public partial class Model
        {
            public Status status { get; set; }
        }

        /**
      * @api {post} /Task/Status/{TaskID} UpdateStatus
      * @apiVersion 0.1.5
      * @apiName UpdateStatus
      * @apiGroup Task
      *
      * @apiParam {GUID} TaskID Task identifier
      * @apiParam {string} Status Task status identifier - Unassigned, Assigned, Done, Pending
      * 
      *@apiSuccess {String} status Task status changed
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       *       {
       *       "status":"StatusChanged"
       *       }
       * 
       * @apiError NotFound Task with specified ID was not found
       * @apiErrorExample Error-Response:
       * HTTP/1.1 404 NotFound
       * {
       *  "status":"notFound"
       * }
       * @apiError InvalidInput Status name is not correct
         *@apiErrorExample Error-Response:
         * HTTP/1.1 400 BadRequest
         * {
         *   "status":"InvalidStatus"
         * }
  */

        [HttpPost("Status/{TaskID}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid TaskID, [FromBody] Model model)
        {
            try
            {
                Models.Entities.Storage.Task task = await _context.LazyLoadTask(TaskID);
                if (task == null) return NotFound(new { status = "TaskNotFound" });
                task.Status = model.status;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { status = "StatusChanged" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { status = "InvalidStatus" });
            }
        }
        /**
     * @api {get} /Task/Status ListStatus
     * @apiVersion 0.1.5
     * @apiName ListAvailableStatus
     * @apiGroup Task
     * @apiDescription
     * Gets all available statuses that can be assigned to task
     * @apiSuccess {Array} Names List of every status by name 
     * @apiSuccessExample Success-Response:
     * HTTP/1.1 200 OK
      *       {
      *       "names":[
      *          "Unassigned",
                 "Assigned",
                  "Pending",
                  "Done"
      *       ]
      *       }
     */
        [HttpGet("Status")]
        public IActionResult ListStatus()
        {
            try
            {
                List<string> names = new List<string>();
                foreach (var status in Enum.GetNames(typeof(Status)))
                {
                    names.Add(status);
                }

                return Ok(names);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "InternalError" });
            }
        }

        /**
          * @api {get} /Task/CustomerTasks CustomerTasks
          * @apiVersion 0.1.5
          * @apiName GetCustomerTasks
          * @apiGroup Task
          * @apiDescription
          * Gets all tasks issued by customer
          * @apiSuccess {Array} tasks List of every issued task 
          * @apiSuccessExample Success-Response:
          * HTTP/1.1 200 OK
           *[
                {
                "taskID": "4f48c274-d49a-4610-aed0-e6f6c828ebbb",
                "describe": "Jesli chcialbys zglosic wymiane recznikow, wybierz te opcje",
                "caseID": "1969c5c3-fad6-4c2c-8d1c-c104f314297c",
                "roomID": "d89fcd07-efba-4ee0-aa10-242c872454d1",
                "room": {
                    "roomID": "d89fcd07-efba-4ee0-aa10-242c872454d1",
                    "number": "1",
                    "userID": "377dcd34-bbff-4bdd-afc8-5e760ef1f1fd",
                    "occupied": true
                },
                 "status": "Assigned"
                }
            ]
    */

        [HttpGet("CustomerTasks")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerTasks()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            Customer customer = await _context.LazyLoadUserByEmail(email);
            var tasks = customer.IssuedTasks?.Select(q => new
            {
                taskID = q.TaskID,
                describe=q.Describe,
                caseID =q.CaseID,
                roomID= q.RoomID,
                room = q.Room,
                status = Enum.GetName(typeof (Status),q.Status)
            }).ToList();
            return Ok(tasks);
        }
    }
}
