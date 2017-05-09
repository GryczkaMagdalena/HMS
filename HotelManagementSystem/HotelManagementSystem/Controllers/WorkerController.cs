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
            }
            catch (Exception)
            {
                return Json(new { status = "notFound" });
            }
            return Json(new
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

        // PUT: api/Worker/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] User value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User origin = await context.Users.FindAsync(id);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = value;
                    origin.Id = id;
                    context.Users.Attach(origin);
                    context.Entry(origin).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Json(new { status = "updated" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = "notFound" });
            }
        }

        //POST: api/Worker
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.Id = value.LastName;
                    await context.Users.AddAsync(value);
                    await context.SaveChangesAsync();
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
        
        //POST: api/Worker/{id}
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(string id)
        {
            User user = null;

            try
            {
                if (ModelState.IsValid)
                {
                    user = await context.Users.FindAsync(id);
                    if (user == null) return Json(new { status = "notFound" });
                    var worker_shift = user.Shifts.Single(v => v.StartTime < DateTime.Today && v.EndTime > DateTime.Today);
                    worker_shift.Break = true;
                    worker_shift.ActualTime.AddMinutes(15);

                    return Json(new { status = "added" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "failure"+ex });
            }
        }


        // DELETE api/Worker/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                User toDelete = await context.Users.FindAsync(id);
                if (toDelete != null)
                {
                    context.Users.Attach(toDelete);
                    context.Entry(toDelete).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    return Json(new { status = "removed" });
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
    }
}