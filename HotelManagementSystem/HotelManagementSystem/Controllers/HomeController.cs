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
using Microsoft.Extensions.Logging;

namespace HotelManagementSystem.Controllers
{
    [EnableCors("HotelCorsPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private IdentityContext _context;
        private ILogger _logger;
        public HomeController(ILogger<HomeController> logger, IdentityContext context)
        {
            _context = context;
            _logger = logger;
        }
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
       *    "ruleID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
       *    "name":"Rule1",
       *    "description":"Rule 1 desc"
       *    }
       * ]
       * 
      */
        [HttpGet]
        public async Task<IActionResult> Rule()
        {
            List<Rule> rules = await _context.Rules.ToListAsync();
            //Alokacja anonimowego obiektu przechowującego dane z obiektu klasy
            //jest to przykład, normalnie zwrócenie samej listy spowoduje auto-parsowanie
            // jednakże czasami chcemy wyłączyć tylko kilka pól
            var rulesObjectified = rules.Select(q => new
            {
                RuleID = q.RuleID,
                Name = q.Name,
                Description = q.Description
            });
            return Ok(rulesObjectified);
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
       *@apiSuccess {String} ruleID Rule identifier
       * @apiSuccess {String} title Rule title
       * @apiSuccess {String} description Rule details
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "ruleID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "title":"ExampleRule",
        *       "description":"Restrict something",
        *       }
        *@apiError NotFound Given ID does not appeal to any of rules
        *@apiErrorExample Error-Response:
        * HTTP/1.1 404 NotFound
        * {
        *   "status":"notFound"
        * }
       */
        // GET api/values/5
        [HttpGet("{RuleID}")]
        public async Task<IActionResult> GetRule(Guid RuleID)
        {
            Rule rule = null;
            try
            {
                rule = await _context.Rules.FindAsync(RuleID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
            }
            return Ok(rule);
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
         * HTTP/1.1 400 BadRequest
         * {
         *   "status":"failure"
         * }
         * @apiError NotFound Given ID does not appeal to any of rules
        *@apiErrorExample Error-Response:
        * HTTP/1.1 404 NotFound
        * {
        *   "status":"notFound"
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
                    await _context.Rules.AddAsync(value);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "created" });
                }
                else
                {
                    return BadRequest(new {status="failure" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
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
        * HTTP/1.1 400 BadRequest
        * {
        *   "status":"failure"
        * }
        * 
        * @apiError NotFound Rule with specified ID was not found
        * @apiErrorExample Error-Response:
        * HTTP/1.1 404 NotFound
        * {
        *  "status":"notFound"
        * }
   */
        // PUT api/values/5
        [HttpPut("{RuleID}")]
        public async Task<IActionResult> Rule([FromRoute] Guid RuleID, [FromBody]Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule origin = await _context.Rules.FindAsync(RuleID);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = value;
                    origin.RuleID = RuleID;
                    _context.Rules.Attach(value);
                    _context.Entry(value).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "updated" });
                }
                else
                {
                    return BadRequest(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new { status = "notFound" });
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
       * HTTP/1.1 404 NotFound
       * {
       *  "status":"notFound"
       * }
       * @apiError BadRequest Given ID does not appeal to any of rules
        *@apiErrorExample Error-Response:
        * HTTP/1.1 400 BadRequest
        * {
        *   "status":"failure"
        * }
  */
        // DELETE api/values/5
        [HttpDelete("{RuleID}")]
        public async Task<IActionResult> Rule(Guid RuleID)
        {
            try
            {
                Rule toDelete = await _context.Rules.FindAsync(RuleID);
                if (toDelete != null)
                {
                    _context.Rules.Attach(toDelete);
                    _context.Entry(toDelete).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                    return Json(new { status = "removed" });
                }
                else
                {
                    return NotFound(new { status = "notFound" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { status = "failure" });
            }
        }
    }
}
