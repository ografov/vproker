using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;
using vproker.Services;

namespace vproker.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<ClientController> Logger { get; set; }

        public ClientService service { get; set; }

        public ClientController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, ClientService service)
        {
            this.service = service;
            Logger = loggerFactory.CreateLogger<ClientController>();
            AppContext = appContext;
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public IActionResult Index()
        {
            var clients = AppContext.Clients.ToList().OrderBy(c => c.Name);
            return View(clients);
        }

        public async Task<ActionResult> Details(string id)
        {
            ClientInfo client = await service.GetInfoById(User, id);
            if (client == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return NotFound();
            }
            return View(client);
        }

        public ActionResult Create()
        {
            //ViewBag.KnowSources = GetKnowledgeSourceListItems();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await service.Store(client);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(client);
        }

        [HttpGet]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public IEnumerable<Client> GetAll()
        {
            return AppContext.Clients.ToList();
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Edit(string id)
        {
            Client client = await FindClientAsync(id);
            if (client == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return NotFound();
            }

            //ViewBag.Items = GetClientsListItems(order.ClientID);
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Update(string id, Client client)
        {
            try
            {
                client.ID = id;
                AppContext.Clients.Attach(client);
                AppContext.Entry(client).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(client);
        }

        private Task<Client> FindClientAsync(string id)
        {
            return AppContext.Clients.SingleOrDefaultAsync(client => client.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> ConfirmDelete(string id, bool? retry)
        {
            Client client = await FindClientAsync(id);
            if (client == null)
            {
                Logger.LogInformation("Delete: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Client client = await FindClientAsync(id);
                AppContext.Clients.Remove(client);
                await AppContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, retry = true });
            }
            return RedirectToAction("Index");
        }

        //private IEnumerable<SelectListItem> GetKnowledgeSourceListItems(string selectedId = null)
        //{
        //    var tmp = AppContext.KnowSources.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

        //    // Create authors list for <select> dropdown
        //    return tmp
        //        .OrderBy(knowItem => knowItem.Name)
        //        .Select(knowItem => new SelectListItem
        //        {
        //            Text = knowItem.Name,
        //            Value = knowItem.ID.ToString(),
        //            Selected = knowItem.ID == selectedId
        //        });
        //}
    }
}
