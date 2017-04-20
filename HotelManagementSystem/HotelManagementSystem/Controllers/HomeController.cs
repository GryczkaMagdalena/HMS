using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Entities.Storage;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private StorageContext storage = new StorageContext();
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Rule()
        {
            List<Rule> rules = await storage.Rules.ToListAsync();
            //Alokacja anonimowego obiektu przechowującego dane z obiektu klasy
            //jest to przykład, normalnie zwrócenie samej listy spowoduje auto-parsowanie
            // jednakże czasami chcemy wyłączyć tylko kilka pól
            var rulesObjectified = rules.Select(q => new
            {
                RuleID = q.RuleID,
                Name = q.Name,
                Description = q.Description
            });
            return Json(rulesObjectified);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRule(Guid id)
        {
            Rule rule = null;
            try
            {
                rule = await storage.Rules.FindAsync(id);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json(rule);
        }
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Rule([FromBody]Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.RuleID = Guid.NewGuid();
                    await storage.Rules.AddAsync(value);
                    await storage.SaveChangesAsync();
                    return Json(new { status = "created" });
                }
                else
                {
                    return Json(new {status="failure" });
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Rule([FromRoute] Guid id, [FromBody]Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    value.RuleID = id;
                    storage.Rules.Attach(value);
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Rule(Guid id)
        {
            try
            {
                Rule toDelete = await storage.Rules.FindAsync(id);
                if (toDelete != null)
                {
                    storage.Rules.Attach(toDelete);
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
