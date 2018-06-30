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
        [HttpGet]
        public JsonResult GetAll()
        {
            return Json(_service.GetAll());
        }

        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(string id)
        {
            return Json(_service.GetById(id));
        }
        
        // POST: api/ToolMaintain
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/ToolMaintain/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
