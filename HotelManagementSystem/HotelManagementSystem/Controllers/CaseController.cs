using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models.Infrastructure;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [EnableCors("HotelCorsPolicy")]
    [Route("api/Case")]
    public class CaseController : Controller
    {
        private IdentityContext _context;
        private ILogger _logger;
        public CaseController(ILogger<CaseController> logger,IdentityContext context)
        {
            _context = context;
            _logger = logger;
        }
        /**
       * @api {get} /Case List
       * @apiVersion 0.1.0
       * @apiName List
       * @apiGroup Case
       *
       *@apiSuccess {Array} cases List of all existing cases
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *   [
        *       {
        *       "caseID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "title":"ExampleCase",
        *       "description":"Clean something",
        *       "workerType":"Technician",
        *       "estimatedTime":"01:00:00"
        *       }
        *   ]
       */
        // GET api/Case
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Case> cases = await _context.Cases.ToListAsync();

            return Ok(
                cases.Select(q => new
                {
                    CaseID = q.CaseID,
                    Description = q.Description,
                    Title = q.Title,
                    WorkerType = Enum.GetName(typeof(WorkerType), q.WorkerType),
                    EstimatedTime = q.EstimatedTime
                })
                );
        }
        /**
  * @api {get} /Case/CaseID Read
  * @apiVersion 0.1.0
  * @apiName Read
  * @apiGroup Case
  *
  * @apiParam {GUID} CaseID Case identifier
  * 
  * 
  *@apiSuccess {String} caseID Case identifier
  * @apiSuccess {String} title Case title
  * @apiSuccess {String} description Case details
  * @apiSuccess {String} workerType Worker type associated with this case 
  * @apiSuccess {TimeSpan} estimatedTime Estimated time needed to perform task with this case
  *@apiSuccessExample Success-Response:
  * HTTP/1.1 200 OK
   *       {
   *       "caseID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
   *       "title":"ExampleCase",
   *       "description":"Clean something",
   *       "workerType":"Technician",
   *       "estimatedTime":"01:00:00"
   *       }
   *@apiError NotFound Given ID does not appeal to any of cases
   *@apiErrorExample Error-Response:
   * HTTP/1.1 404 NotFound
   * {
   *   "status":"notFound"
   * }
  */
        // GET api/Case/{id}
        [HttpGet("{CaseID}")]
        public async Task<IActionResult> Read(Guid CaseID)
        {
            Case value = null;
            try
            {
                value = await _context.Cases.FindAsync(CaseID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new {status="notFound" });
            }
            return Ok(new {
                CaseID = value.CaseID,
                Title = value.Title,
                Description = value.Description,
                WorkerType = Enum.GetName(typeof(WorkerType),value.WorkerType),
                EstimatedTime = value.EstimatedTime
            });
        }


        /**
          * @api {get} /Case/Filter/WorkerType ReadFilter
          * @apiVersion 0.1.2
          * @apiName ReadFilter
          * @apiGroup Case
          *
          * @apiParam {String} WorkerType (Cleaner or Technician)
          * 
          * 
          *@apiSuccess {String} caseID Case identifier
          * @apiSuccess {String} title Case title
          * @apiSuccess {String} description Case details
          * @apiSuccess {String} workerType Worker type associated with this case 
          * @apiSuccess {TimeSpan} estimatedTime Estimated time of case
          *@apiSuccessExample Success-Response:
          * HTTP/1.1 200 OK
           *       {
           *       "caseID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
           *       "title":"ExampleCase",
           *       "description":"Clean something",
           *       "workerType":"Technician",
           *        "estimatedTime":"01:00:00"
           *       }
           *@apiError NotFound Given ID does not appeal to any of cases
           *@apiErrorExample Error-Response:
           * HTTP/1.1 404 NotFound
           * {
           *   "status":"notFound"
           * }
          */
        // GET api/Case/Filter/{Type}
        [HttpGet("Filter/{Type}")]
        public async Task<IActionResult> Read(String Type) {
            List<Case> cases = null;
            try 
            {
                cases = await _context.Cases.Where(x => x.WorkerType.ToString() == Type).ToListAsync();
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(new {status="notFound"});
            }
            return Ok(new {
                cases = cases.Select(q => new
                {
                    CaseID = q.CaseID,
                    Description = q.Description,
                    Title = q.Title,
                    WorkerType = Enum.GetName(typeof(WorkerType), q.WorkerType),
                    EstimatedTime = q.EstimatedTime
                })
            });
        }

        /**
         * @api {post} /Case Create
         * @apiVersion 0.1.0
         * @apiName Create
         * @apiGroup Case
         *
         * @apiParam {String} Title Case title
         * @apiParam {String} Description Case details
         * @apiParam {Number} WorkerType Type of case - one of (0-Cleaner,1-Technician,2-None)
         * @apiParam {String} EstimatedTime Time in format "HH:mm:ss"
         * 
         * 
         *@apiSuccess {String} status Case was created 
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
     */
        //Post api/Case
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Case value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    value.CaseID = Guid.NewGuid();
                    await _context.Cases.AddAsync(value);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = "created" });
                }
                else
                {
                    return BadRequest(new { status = "failure" });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new { status = "failure" });
            }
        }
        /**
        * @api {put} /Case?CaseID Update
        * @apiVersion 0.1.0
        * @apiName Update
        * @apiGroup Case
        *
        * @apiParam {GUID} CaseID Case identifier
        * @apiParam {String} Title Case title
        * @apiParam {String} Description Case details
        * @apiParam {Number} WorkerType Type of case - one of (0-Cleaner,1-Technician,2-None)
         * @apiParam {String} EstimatedTime Time in format "HH:mm:ss"
        * 
        * 
        *@apiSuccess {String} status Case was updated 
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
         * @apiError NotFound Case with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 404 NotFound
         * {
         *  "status":"notFound"
         * }
    */
        // PUT api/Case/{id}
        [HttpPut("{CaseID}")]
        public async Task<IActionResult> Update([FromRoute] Guid CaseID,[FromBody] Case value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Case origin = await _context.Cases.FindAsync(CaseID);
                    if (origin == null) return NotFound(new { status = "notFound" });
                    origin = value;
                    origin.CaseID = CaseID;
                    _context.Cases.Attach(origin);
                    _context.Entry(origin).State = EntityState.Modified;
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
                return BadRequest(new { status = "failure" });
            }
        }
        /**
       * @api {delete} /Case/CaseID Delete
       * @apiVersion 0.1.0
       * @apiName Delete
       * @apiGroup Case
       *
       * @apiParam {GUID} CaseID Case identifier
       * 
       * 
       *@apiSuccess {String} status Case was deleted
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "status":"removed"
        *       }
        * 
        * @apiError NotFound Case with specified ID was not found
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
        // DELETE api/Case/{id}
        [HttpDelete("{CaseID}")]
        public async Task<IActionResult> Delete (Guid CaseID)
        {
            try
            {
                Case toDelete = await _context.Cases.FindAsync(CaseID);
                if (toDelete != null)
                {
                    _context.ChangeTracker.DetectChanges();
                    _context.Cases.Attach(toDelete);
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
