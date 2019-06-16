using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

public class ToolModel
{
    public string ID { get; set; }
    public string Name { get; set; }
}
namespace vproker.Controllers.ApiControllers
{
    [Route("api/tool")]
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
        [HttpGet]
        public JsonResult GetAll()
        {
            var tools = AppContext.Tools;
            return Json(tools);
        }

        [AllowAnonymous]
        [HttpGet("models")]
        public JsonResult GetModels()
        {
            // IEnumerable<(string id, string name)> tuples = AppContext.Tools.Select(x => (x.ID, x.Name));
            // List<(string id, string name)> toolNames = new List<(string id, string name)>();
            // foreach (var tool in AppContext.Tools)
            // {
            //     toolNames.Add((id: tool.ID, name: tool.Name));
            // }
            // return Json(toolNames.ToArray());
            List<ToolModel> tools = new List<ToolModel>();
            foreach (var tool in AppContext.Tools)
            {
                tools.Add(new ToolModel() { ID = tool.ID, Name = tool.Name });
            }
            return Json(tools.ToArray());            
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            Tool tool = await AppContext.Tools.SingleOrDefaultAsync(b => b.ID == id);
            return Json(tool);
        }

        [AllowAnonymous]
        [HttpPost]
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
