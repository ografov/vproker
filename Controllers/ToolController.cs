﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vproker.Controllers
{
    [Authorize(Roles = AuthData.ADMIN_ROLE)]
    public class ToolController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }
        public ILogger<ToolController> Logger { get; set; }

        public ToolController(ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            AppContext = context;
            Logger = loggerFactory.CreateLogger<ToolController>();
        }

        public IActionResult Index()
        {
            var tools = AppContext.Tools;
            return View(tools);
        }

        public async Task<ActionResult> Details(string id)
        {
            Tool tool = await AppContext.Tools.SingleOrDefaultAsync(b => b.ID == id);
            if (tool == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return NotFound();
            }
            return View(tool);
        }

        public ActionResult Create()
        {
            return View(new Tool());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tool tool)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppContext.Tools.Add(tool);
                    await AppContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }
            return View(tool);
        }

        public async Task<ActionResult> Edit(string id)
        {
            Tool tool = await FindToolAsync(id);
            if (tool == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return NotFound();
            }

            if(tool.Purchased == DateTime.MinValue)
            {
                tool.Purchased = Tool.DefaultPurchasedDate;
            }
            return View(tool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(string id, Tool tool)
        {
            try
            {
                tool.ID = id;
                AppContext.Tools.Attach(tool);
                AppContext.Entry(tool).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }
            return View(tool);
        }

        private Task<Tool> FindToolAsync(string id)
        {
            return AppContext.Tools.SingleOrDefaultAsync(tool => tool.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(string id, bool? retry)
        {
            Tool tool = await FindToolAsync(id);
            if (tool == null)
            {
                Logger.LogInformation("Delete: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(tool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Tool tool = await FindToolAsync(id);
                AppContext.Tools.Remove(tool);
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
