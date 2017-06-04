using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Cors;
using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.Extensions.Logging;
using HotelManagementSystem.Models.Helpers;
using HotelManagementSystem.Models.Abstract;
using System.Threading;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Worker")]
    public class WorkerController : Controller
    {
        private IdentityContext _context;
        private ILogger _logger;
        private TaskDisposer _taskDisposer;
        private IUserService _userService;
        public WorkerController(IdentityContext context, ILogger<WorkerController> logger,IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _taskDisposer = new TaskDisposer(_userService, _context);

        }
        // GET: api/Worker
        /**
       * @api {get} /Worker/ List
       * @apiVersion 0.1.3
       * @apiName List
       * @apiGroup Worker
       */
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Worker> users = await _context.LazyLoadWorkers();
            return Ok(users.Select(q => new
            {
                Id = q.Id,
                FirstName = q.FirstName,
                LastName = q.LastName,
                WorkerType = q.WorkerType,
                ReceivedTasks = q.ReceivedTasks,
                Shifts = q.Shifts,
            }));
        }
        /**
        * @api {get} /Worker/{id} Read
        * @apiVersion 0.1.3
        * @apiName Read
        * @apiGroup Worker
        */ 
        // GET: api/Worker/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read([FromRoute] string id)
        {
            Worker user = null;

            try
            {
                user = await _context.LazyLoadWorker(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
            return Ok(new
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                WorkerType = user.WorkerType,
                ReceivedTasks = user.ReceivedTasks,
                Shifts = user.Shifts,
            });
        }

        /**
        * @api {get} /Worker/{id} Tasks
        * @apiVersion 0.1.3
        * @apiName Read
        * @apiGroup Worker
        */
        // GET: api/Worker/Tasks/{id}
        [HttpGet("Tasks/{id}")]
        public async Task<IActionResult> Tasks([FromRoute] string id)
        {
            Worker user = null;
            //List<User> tasks = null;

            try
            {
                user = await _context.LazyLoadWorker(id);
                var tasks = user.ReceivedTasks.Select(q => new
                      {
                        TaskID = q.TaskID,
                        Describe = q.Describe,
                        RoomID = q.RoomID,
                        Listener = q.Listener,
                        Issuer = q.Issuer.ToJson(),
                        Case = q.Case,
                }
                ).OrderBy(q => q.Describe);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
        }

        /**
       * @api {get} /Worker/Shifts ActualizeShifts
       * @apiVersion 0.1.3
       * @apiName ActualizeShifts
       * @apiGroup Worker
       *
       * *@apiSuccess {String} status Shifts were updated
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "status":"success"
        *       }
        * @apiError {String} status Shifts are acutal - no need to update
        * @apiErrorExample Error-Response
        * HTTP/1.1 400 BadRequest
        * {
        *    "status":"this action is not needed"
        * }
        * 
        */
        [HttpGet("shifts")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ActualizeShifts()
        {
            var result = await DbInitializer.AddWorkerShifts(_context);
            if (result) return Ok(new { status = "success" });
            return BadRequest(new { status = "this action is not needed" });
        }

    }
}
