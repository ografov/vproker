using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace vproker.Controllers
{
    [Authorize(Roles = AuthData.ADMIN_ROLE)]
    public class SettingsController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<SettingsController> Logger { get; set; }

        public SettingsController(ILoggerFactory loggerFactory, ApplicationDbContext appContext)
        {
            Logger = loggerFactory.CreateLogger<SettingsController>();
            AppContext = appContext;
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<IActionResult> Index()
        {
            Settings settings = await GetSettingsAsync();
            if (settings == null)
            {
                settings = new Settings();
            }

            return View(settings);
        }

        private Task<Settings> GetSettingsAsync()
        {
            return AppContext.Settings.FirstOrDefaultAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public ActionResult Update(Settings model)
        {
            try
            {
                Store(model);
                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
                return RedirectToAction("Index", "Order");
            }
        }

        [HttpPost]
        public ActionResult AddPartner(Settings model)
        {
            model.PartnersList.Add(new Partners(model.PartnerName, model.PartnerDiscount));
            //Store(model);
            return RedirectToAction("Index", "Settings");
        }
        private void Store(Settings settings)
        {
            settings.BeforeStore();
            if (AppContext.Settings.Any())
            {
                AppContext.Settings.Attach(settings);
                AppContext.Entry(settings).State = EntityState.Modified;
            }
            else
            {
                AppContext.Settings.Add(settings);
            }

            AppContext.SaveChanges();
        }
    }
}
