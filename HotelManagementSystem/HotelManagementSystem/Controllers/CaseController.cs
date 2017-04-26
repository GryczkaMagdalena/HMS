using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/Case")]
    public class CaseController : Controller
    {
        private StorageContext storage = new StorageContext();

        // GET api/Case
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Case> cases = await storage.Cases.ToListAsync();
            return Json(cases);
        }

        // GET api/Case/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(Guid id)
        {
            Case value = null;
            try
            {
                value = await storage.Cases.FindAsync(id);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(value);
        }

        //Post api/Case
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Case value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.CaseID = Guid.NewGuid();
                    await storage.Cases.AddAsync(value);
                    await storage.SaveChangesAsync();
                    return Json(new { status = "created" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
            }
        }

        // PUT api/Case/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] Case value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.CaseID = id;
                    storage.Cases.Attach(value);
                    storage.Entry(value).State = EntityState.Modified;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "updated" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
            }
        }

        // DELETE api/Case/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (Guid id)
        {
            try
            {
                Case toDelete = await storage.Cases.FindAsync(id);
                if (toDelete != null)
                {
                    storage.Cases.Attach(toDelete);
                    storage.Entry(toDelete).State = EntityState.Deleted;
                    await storage.SaveChangesAsync();
                    return Json(new { status = "removed" });
                }
                else
                {
                    return Json(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                return Json(ex);
            }
        }
    }
}
