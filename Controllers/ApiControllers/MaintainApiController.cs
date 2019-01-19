using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vproker.Models;
using vproker.Services;

namespace vproker.Controllers.ApiControllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/maintain")]
    public class MaintainApiController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<MaintainApiController> Logger { get; set; }

        private MaintainService _service { get; set; }

        public MaintainApiController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, MaintainService service)
        {
            _service = service;
            Logger = loggerFactory.CreateLogger<MaintainApiController>();
            AppContext = appContext;
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public JsonResult GetAll()
        {
            return Json(_service.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("getCurrent")]
        public JsonResult GetCurrent()
        {
            return Json(_service.GetCurrent());
        }

        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(string id)
        {
            return Json(_service.GetById(id));
        }
        
        // POST: api/ToolMaintain
        [HttpPost]
        public async Task<ActionResult> Store([FromBody]Maintain item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var savedStore = await _service.Store(item);
                    return Json(savedStore);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
                return StatusCode(500);
            }
        }
        
        //// PUT: api/ToolMaintain/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
