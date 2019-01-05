﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vproker.Controllers.ApiControllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = AuthData.ADMIN_ROLE)]
    public class ToolApiController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }
        public ILogger<ToolController> Logger { get; set; }

        public ToolApiController(ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            AppContext = context;
            Logger = loggerFactory.CreateLogger<ToolController>();
        }

        [AllowAnonymous]
        [HttpGet("/api/[controller]")]
        public JsonResult GetAll()
        {
            var tools = AppContext.Tools;
            return Json(tools);
        }

        [AllowAnonymous]
        [HttpGet("/api/[controller]/{id}")]
        public async Task<JsonResult> Get(string id)
        {
            Tool tool = await AppContext.Tools.SingleOrDefaultAsync(b => b.ID == id);
            return Json(tool);
        }

        [AllowAnonymous]
        [HttpPost("/api/[controller]")]
        public async Task<ActionResult> Store([FromBody]Tool tool)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppContext.Tools.Add(tool);
                    await AppContext.SaveChangesAsync();

                    tool = AppContext.Tools.Find(tool.ID);
                    return Json(tool);
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
    }
}
