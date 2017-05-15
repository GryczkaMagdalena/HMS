using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [EnableCors("HotelCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Worker")]
    public class WorkerController : Controller
    {
        private IdentityContext context = new IdentityContext();

        // GET: api/Worker
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<User> users = await context.Users.ToListAsync();

            return Json(users.Select(q => new
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

        // GET: api/Worker/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read([FromRoute] string id)
        {
            User user = null;

            try
            {
                user = await context.Users.FindAsync(id);
                if (user == null) throw new Exception();
            }
            catch (Exception)
            {
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
            var result = await DbInitializer.AddWorkerShifts(context);
            if (result) return Ok(new { status = "success" });
            return BadRequest(new { status = "this action is not needed" });
        }
    }
}
