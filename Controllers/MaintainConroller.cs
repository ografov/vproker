using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using vproker.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vproker.Controllers
{
    //[Route("[controller]/[action]")]
    [Authorize]
    public class MaintainController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }
        public ILogger<MaintainController> Logger { get; set; }
        private MaintainService _service { get; set; }

        public MaintainController(ILoggerFactory loggerFactory, ApplicationDbContext context, MaintainService service)
        {
            AppContext = context;
            Logger = loggerFactory.CreateLogger<MaintainController>();
            _service = service;
        }

        public IActionResult Index()
        {
            var maintains = _service.GetAll();
            maintains = maintains.OrderBy(o => o.Name);
            return View(maintains);
        }

        public async Task<ActionResult> Details(string id)
        {
            Maintain maintain = await FindMaintainAsync(id);
            if (maintain == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return NotFound();
            }
            return View(maintain);
        }

        public ActionResult Create()
        {
            ViewBag.Tools = ToolService.GetToolsListItems(AppContext.Tools.ToList(), optional: true);
            return View(new Maintain());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Maintain maintain)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppContext.Maintains.Add(maintain);
                    await AppContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }
            return View(maintain);
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Edit(string id)
        {
            Maintain maintain = await FindMaintainAsync(id);
            if (maintain == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return NotFound();
            }

            ViewBag.Tools = ToolService.GetToolsListItems(AppContext.Tools.ToList(), optional: true);
            return View(maintain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Update(string id, Maintain maintain)
        {
            try
            {
                maintain.ID = id;
                AppContext.Maintains.Attach(maintain);
                AppContext.Entry(maintain).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }
            return View(maintain);
        }

        private Task<Maintain> FindMaintainAsync(string id)
        {
            return AppContext.Maintains.Include(m => m.Tool).SingleOrDefaultAsync(maintain => maintain.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> ConfirmDelete(string id, bool? retry)
        {
            Maintain maintain = await FindMaintainAsync(id);
            if (maintain == null)
            {
                Logger.LogInformation("Delete: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(maintain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Maintain maintain = await FindMaintainAsync(id);
                AppContext.Maintains.Remove(maintain);
                await AppContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, retry = true });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(string id)
        {
            try
            {
                Maintain maintain = await FindMaintainAsync(id);
                maintain.FinishedDate = DateTime.UtcNow;

                AppContext.Maintains.Attach(maintain);
                AppContext.Entry(maintain).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Close", new { id = id, retry = true });
            }
        }

        [HttpGet]
        [ActionName("Close")]
        public async Task<ActionResult> ConfirmClose(string id, bool? retry)
        {
            Maintain maintain = await FindMaintainAsync(id);
            if (maintain.FinishedDate != null)
            {
                throw new Exception("Обслуживание не может быть закрыто дважды. Дата закрытия обслуживания уже указана - " + maintain.FinishedDate);
            }
            if (maintain == null)
            {
                Logger.LogInformation("Close: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(new CloseMaintainModel(maintain));
        }
    }
}
