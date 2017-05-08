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
        /**
       * @api {get} /Task List
       * @apiVersion 0.1.0
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
             /**
       * @api {get} /Task?TaskID Read
       * @apiVersion 0.1.0
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
                rule = await storage.Tasks.FindAsync(id);
            }
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
            return Json(rule);
        }
        /**
        * @api {put} /task?TaskID Update
        * @apiVersion 0.1.0
        * @apiName Update
        * @apiGroup task
        *
        // * @apiParam {GUID} taskID task identifier
        // * @apiParam {String} Title task title
        // * @apiParam {String} Description task details
        // * @apiParam {Number} WorkerType Type of task - one of (0-Cleaner,1-Technician,2-None)
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
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
        }
        /**
         * @api {post} /Task Create
         * @apiVersion 0.1.0
         * @apiName Create
         * @apiGroup Task
         *
         // * @apiParam {String} Title task title
         // * @apiParam {String} Description task details
         // * @apiParam {Number} WorkerType Type of task - one of (0-Cleaner,1-Technician,2-None)
         * 
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
        public async Task<IActionResult> Create([FromBody] Models.Entities.Storage.Task value)
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
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
        }

        /**
       * @api {delete} /Task?TaskID Delete
       * @apiVersion 0.1.0
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