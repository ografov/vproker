using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace vproker.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = AuthData.ADMIN_ROLE)]
    public class MaintainController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }
        public ILogger<MaintainController> Logger { get; set; }

        public MaintainController(ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            AppContext = context;
            Logger = loggerFactory.CreateLogger<MaintainController>();
        }

        public IActionResult Index()
        {
            return View(AppContext.Maintains);
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
            return View();
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

        public async Task<ActionResult> Edit(string id)
        {
            Maintain maintain = await FindMaintainAsync(id);
            if (maintain == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return NotFound();
            }

            return View(maintain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return AppContext.Maintains.SingleOrDefaultAsync(maintain => maintain.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
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
    }
}
