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

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                PassNotFinishedTasks();
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }).Start();
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
            List<User> users = await _context.LazyLoadUsers();
            return Ok(users.Select(q => new
            {
                Id = q.Id,
                FirstName = q.FirstName,
                LastName = q.LastName,
                Room = q.Room,
                WorkerType = q.WorkerType,
                IssuedTasks = q.IssuedTasks,
                ListenedTasks = q.ListenedTasks,
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
            User user = null;

            try
            {
                user = await _context.LazyLoadUser(id);
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
                Room = user.Room,
                WorkerType = user.WorkerType,
                IssuedTasks = user.IssuedTasks,
                ListenedTasks = user.ListenedTasks,
                ReceivedTasks = user.ReceivedTasks,
                Shifts = user.Shifts,
            });
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

        public async void PassNotFinishedTasks()
        {
            foreach(var worker in await _context.LazyLoadWorkers())
            {
                if (worker.ReceivedTasks.Any() && worker.CurrentShift() == null)
                {
                    // Here status of tasks should be checked as well
                    List<KeyValuePair<Models.Entities.Storage.Task, User>> tasksToRemove =
                        new List<KeyValuePair<Models.Entities.Storage.Task, User>>();
                    
                    foreach(var unifinishedTask in worker.ReceivedTasks)
                    {
                        var newWorker = await _taskDisposer.FindWorker(unifinishedTask.Case);
                        if (newWorker != null)
                        {
                            tasksToRemove
                                .Add(new KeyValuePair<Models.Entities.Storage.Task, User>(unifinishedTask,newWorker));
                        }
                    }

                    foreach (var pair in tasksToRemove)
                    {
                        pair.Value.ReceivedTasks.Add(pair.Key);
                        pair.Key.Receiver = pair.Value;
                        _context.Entry(pair.Value).State = EntityState.Modified;
                        _context.Entry(pair.Key).State = EntityState.Modified;
                        worker.ReceivedTasks.Remove(pair.Key);
                        _context.Entry(worker).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Task moved to another worker", pair);
                    }
                }
            }
        }
    }
}
