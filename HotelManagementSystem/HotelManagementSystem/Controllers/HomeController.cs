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
    //[Authorize]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private StorageContext storage = new StorageContext();
        // GET api/values

        /**
      * @api {get} /Home List
      * @apiVersion 0.1.0
      * @apiName List
      * @apiGroup Home
      *
      *@apiSuccess {Array} rules List of all rules
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       * [
       *    { 
       *    "RuleID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
       *    "Name":"Rule1",
       *    "Description":"Rule 1 desc"
       *    }
       * ]
       * 
      */
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
           /**
       * @api {get} /Home?RuleID Read
       * @apiVersion 0.1.0
       * @apiName Read
       * @apiGroup Home
       *
       * @apiParam {GUID} RuleID Rule identifier
       * 
       * 
       *@apiSuccess {String} RuleID Rule identifier
       * @apiSuccess {String} Title Rule title
       * @apiSuccess {String} Description Rule details
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "RuleID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Title":"ExampleRule",
        *       "Description":"Restrict something",
        *       }
        *@apiError NotFound Given ID does not appeal to any of rules
        *@apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
        * {
        *   "status":"notFound"
        * }
       */
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRule(Guid RuleID)
        {
            Rule rule = null;
            try
            {
                rule = await storage.Rules.FindAsync(RuleID);
            }
            catch (Exception)
            {
                return Json(new { status = "notFound" });
            }
            return Json(rule);
        }

        /**
        * @api {post} /Home Create
        * @apiVersion 0.1.0
        * @apiName Create
        * @apiGroup Home
        *
        * @apiParam {String} Title Rule title
        * @apiParam {String} Description Rule details
        * 
        * 
        *@apiSuccess {String} status Rule was created 
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
        /**
       * @api {put} /Home?RuleID Update
       * @apiVersion 0.1.0
       * @apiName Update
       * @apiGroup Home
       *
       * @apiParam {GUID} RuleID Rule identifier
       * @apiParam {String} Title Rule title
       * @apiParam {String} Description Rule details
       * 
       *@apiSuccess {String} status Rule was updated 
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
        * @apiError NotFound Rule with specified ID was not found
        * @apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
        * {
        *  "status":"notFound"
        * }
   */
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Rule([FromRoute] Guid RuleID, [FromBody]Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule origin = await storage.Rules.FindAsync(RuleID);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = value;
                    origin.RuleID = RuleID;
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
        /**
      * @api {delete} /Home?RuleID Delete
      * @apiVersion 0.1.0
      * @apiName Delete
      * @apiGroup Home
      *
      * @apiParam {GUID} RuleID Rule identifier
      * 
      * 
      *@apiSuccess {String} status Rule was deleted
      *@apiSuccessExample Success-Response:
      * HTTP/1.1 200 OK
       *       {
       *       "status":"removed"
       *       }
       * 
       * @apiError NotFound Rule with specified ID was not found
       * @apiErrorExample Error-Response:
       * HTTP/1.1 200 OK
       * {
       *  "status":"notFound"
       * }
  */
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Rule(Guid RuleID)
        {
            try
            {
                Rule toDelete = await storage.Rules.FindAsync(RuleID);
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
