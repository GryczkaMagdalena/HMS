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
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Cors;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [EnableCors("HotelCorsPolicy")]
    [Route("api/Case")]
    public class CaseController : Controller
    {
        private StorageContext storage = new StorageContext();

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
        *       "CaseID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Title":"ExampleCase",
        *       "Description":"Clean something",
        *       "WorkerType":"Technician"
        *       }
        *   ]
       */
        // GET api/Case
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Case> cases = await storage.Cases.ToListAsync();

            return Json(
                cases.Select(q => new
                {
                    CaseID = q.CaseID,
                    Description = q.Description,
                    Title = q.Title,
                    WorkerType = Enum.GetName(typeof(WorkerType), q.WorkerType)
                })
                );
        }
             /**
       * @api {get} /Case?CaseID Read
       * @apiVersion 0.1.0
       * @apiName Read
       * @apiGroup Case
       *
       * @apiParam {GUID} CaseID Case identifier
       * 
       * 
       *@apiSuccess {String} CaseID Case identifier
       * @apiSuccess {String} Title Case title
       * @apiSuccess {String} Description Case details
       * @apiSuccess {String} WorkerType Worker type associated with this case 
       *@apiSuccessExample Success-Response:
       * HTTP/1.1 200 OK
        *       {
        *       "CaseID":"4ba83f3c-4ea4-4da4-9c06-e986a8273800",
        *       "Title":"ExampleCase",
        *       "Description":"Clean something",
        *       "WorkerType":"Technician"
        *       }
        *@apiError NotFound Given ID does not appeal to any of cases
        *@apiErrorExample Error-Response:
        * HTTP/1.1 200 OK
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
                value = await storage.Cases.FindAsync(CaseID);
            }
            catch (Exception)
            {
                return Json(new {status="notFound" });
            }
            return Json(new {
                CaseID = value.CaseID,
                Title = value.Title,
                Description = value.Description,
                WorkerType = Enum.GetName(typeof(WorkerType),value.WorkerType) 
            });
        }


        // GET api/Case/{type}
        [HttpGet("{type}")]
        public async Task<IActionResult> Read(WorkerType Type) {
            List<Case> cases = null;
            try 
            {
                cases = await storage.Cases.Where(x => x.WorkerType == Type).ToListAsync();
            } 
            catch (Exception) 
            {
                return Json(new {status="notFound"});
            }
            return Json(new {
                cases = cases.Select(q => new
                {
                    CaseID = q.CaseID,
                    Description = q.Description,
                    Title = q.Title,
                    WorkerType = Enum.GetName(typeof(WorkerType), q.WorkerType)
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
          * HTTP/1.1 200 OK
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
         * HTTP/1.1 200 OK
         * {
         *   "status":"failure"
         * }
         * 
         * @apiError NotFound Case with specified ID was not found
         * @apiErrorExample Error-Response:
         * HTTP/1.1 200 OK
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
                    Case origin = await storage.Cases.FindAsync(CaseID);
                    if (origin == null) return Json(new { status = "notFound" });
                    origin = value;
                    origin.CaseID = CaseID;
                    storage.Cases.Attach(origin);
                    storage.Entry(origin).State = EntityState.Modified;
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
       * @api {delete} /Case?CaseID Delete
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
        * HTTP/1.1 200 OK
        * {
        *  "status":"notFound"
        * }
   */
        // DELETE api/Case/{id}
        [HttpDelete("{CaseID}")]
        public async Task<IActionResult> Delete (Guid CaseID)
        {
            try
            {
                Case toDelete = await storage.Cases.FindAsync(CaseID);
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
