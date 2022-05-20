﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vproker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using vproker.Services;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using System.Security.Claims;

namespace vproker.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<OrderController> Logger { get; set; }

        private OrderService orderService { get; set; }
        private ClientService clientService { get; set; }
		public IConfiguration Configuration { get; }

		public OrderController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, OrderService orderService, ClientService clientService, IConfiguration configuration)
		{
			this.orderService = orderService;
			this.clientService = clientService;
			Configuration = configuration;
			Logger = loggerFactory.CreateLogger<OrderController>();
			AppContext = appContext;
		}

		public IActionResult Index()
        {
            //return User.Identity.Name == AuthData.ADMIN_ID ? History() : ActiveOrders();
            return ActiveOrders();
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public IActionResult History(string sortOrder = "", string searchString = "", DateTime? start = null, DateTime? end = null)
        {
            var orders = new Order[0];

            if (AppContext.Orders.Count() > 0)
            {
                orders = AppContext.Orders.Include(o => o.Tool).Include(o => o.Client).ToArray();

                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ToolSortParm = sortOrder == "Tool" ? "tool_desc" : "Tool";

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o => o.Client.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.Client.Name).ToArray();
                        break;
                    case "Tool":
                        orders = orders.OrderBy(o => o.Tool.Name).ToArray();
                        break;
                    case "tool_desc":
                        orders = orders.OrderByDescending(o => o.Tool.Name).ToArray();
                        break;
                    default: //name ascending
                        orders = orders.OrderBy(s => s.Client.Name).ToArray();
                        break;
                }
                orders = orders.Where(o => o.IsClosed).ToArray();
                if (start != null && end != null)
                {
                    orders = orders.Where(o => o.StartDate >= start.Value && o.EndDate <= end.Value).ToArray();
                }
                else
                {
                    if (start != null)
                        orders = orders.Where(o => o.StartDate >= start.Value).ToArray();
                    if (end != null)
                        orders = orders.Where(o => o.EndDate <= end.Value).ToArray();
                }
            }

            return View("History", orders);
        }

        [Authorize(Roles = AuthData.USER_ROLE)]
        public IActionResult ActiveOrders(string sortOrder = "", string searchString = "")
        {
            var orders = new Order[0];

            if (AppContext.Orders.Count() > 0)
            {
                // sort and filter
                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ToolSortParm = sortOrder == "tool" ? "tool_desc" : "tool";
                ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";

                Order[] todayClosedOrders = orderService.GetTodayClosedOrders(User);
                ViewBag.TodayIncome = todayClosedOrders.Sum(o => o.Payment.HasValue ? o.Payment.GetValueOrDefault() : 0);

                orders = orderService.GetActiveOrders(User, sortOrder, searchString).ToArray();
            }

            return View("ActiveOrders", orders);
        }

        public async Task<ActionResult> Details(string id)
        {
            Order order = await orderService.GetById(id);

            if (order == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return NotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            ViewBag.Tools = ToolService.GetToolsListItems(AppContext.Tools.ToList(), optional: true);

            return View(new CreateOrderModel() { ContractNumber = orderService.SuggestContractNumber().ToString() });
        }


        private IEnumerable<SelectListItem> GetClientListItems(string selectedId = null)
        {
            var tmp = AppContext.Clients.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

            return tmp
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.ID,
                    Selected = t.ID == selectedId
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrderModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Save(User, AppContext);
                    await AppContext.SaveChangesAsync();
                    NotifyClientByWhatsApp(model);
                    return RedirectToAction("ActiveOrders");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }

            ViewBag.Tools = ToolService.GetToolsListItems(AppContext.Tools.ToList(), optional: true);
            return View(model);
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Edit(string id, string backPage)
        {
            Order order = await FindOrderAsync(id);
            if (order == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return NotFound();
            }

            ViewBag.Tools = ToolService.GetToolsListItems(AppContext.Tools.ToList(), optional: false, selectedId: order.ToolID);

            ViewBag.backPage = backPage;

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> Update(string id, Order newOrder, string backPage)
        {
            try
            {
                Order order = AppContext.Orders.SingleOrDefault(o => o.ID == id);
                if (order == null)
                {
                    Logger.LogInformation("Update: Item not found {0}", id);
                    return NotFound();
                }

                order.ToolID = newOrder.ToolID;
                order.Description = newOrder.Description;
                order.CloseDescription = newOrder.CloseDescription;
                order.Usage = newOrder.Usage;
                order.PaidPledge = newOrder.PaidPledge;
                order.Price = newOrder.Price;
                order.StartDate = newOrder.StartDate;
                order.EndDate = newOrder.EndDate;
                order.Payment = newOrder.Payment;
                order.ShouldCallClient = newOrder.ShouldCallClient;

                //AppContext.Orders.Attach(order);
                AppContext.Entry(order).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();

                // TODO: redirect to previous page
                return RedirectToAction(backPage ?? "History");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }
            return View(newOrder);
        }

        private Task<Order> FindOrderAsync(string id)
        {
            return AppContext.Orders.Include(o => o.Tool).Include(o => o.Client).SingleOrDefaultAsync(order => order.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        public async Task<ActionResult> ConfirmDelete(string id, bool? retry)
        {
            Order order = await FindOrderAsync(id);
            if (order == null)
            {
                Logger.LogInformation("Delete: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Order order = await FindOrderAsync(id);
                AppContext.Orders.Remove(order);
                await AppContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, retry = true });
            }
            return RedirectToAction("History");
        }

        [HttpGet]
        [ActionName("Close")]
        public async Task<ActionResult> ConfirmClose(string id, bool? retry)
        {
            Order order = await FindOrderAsync(id);
            if (order.EndDate != null)
            {
                throw new Exception("Заказ не может быть закрыт дважды. Дата закрытия заказа уже указана - " + order.EndDate);
            }
            if (order == null)
            {
                Logger.LogInformation("Close: Item not found {0}", id);
                return NotFound();
            }
            ViewBag.Retry = retry ?? false;
            
            bool isRegularClient = this.clientService.IsRegularClient(User, order.ClientID);
            return View(new CloseOrderModel(order, isRegularClient));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(string id, CloseOrderModel model)
        {
            try
            {
                Order order = await FindOrderAsync(id);
                order.Payment = model.TotalPayment;
                order.CloseDescription = model.CloseDescription;
                order.EndDate = DateTime.UtcNow;
                order.ShouldCallClient = model.ShouldCallClient;

                AppContext.Orders.Attach(order);
                AppContext.Entry(order).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("ActiveOrders");
            }
            catch (Exception)
            {
                return RedirectToAction("Close", new { id = id, retry = true });
            }
        }

        public FileResult DownloadHistory(string start = "", string end = "", string searchString = "")
        {
            string fileName = "vproker-history.csv";
            byte[] fileBytes = orderService.GetHistoryReport(User, start, end, searchString);

            return File(fileBytes, "text/csv; charset=UTF8", fileName); 
        }

        public FileResult DownloadStatsByDays(string start = "", string end = "", string searchString = "")
        {
            string fileName = "vproker_stats-by-days.csv";
            byte[] fileBytes = orderService.GetStatsByDaysReport(User, start, end, searchString);

            return File(fileBytes, "text/csv; charset=UTF8", fileName);
        }

        private void NotifyClientByWhatsApp(CreateOrderModel model)
		{
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{Configuration["WhatsAppMessageRequestUrl"]}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            Logger.LogInformation($"{httpWebRequest.Method} {httpWebRequest.RequestUri.AbsoluteUri}");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string message = $"Номер договора: {model.ContractNumber}"
                    + $"\\nЗалог: {model.PaidPledge}";

                string json = $"{{\"phone\":\"{model.PhoneNumber}\"," +
                              $"\"message\":\"{message}\"}}";

                streamWriter.Write(json);
            }

			try
			{
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
			catch (WebException we)
			{
                using (var stream = we.Response.GetResponseStream()) 
                using (var reader = new StreamReader(stream)) {
                    Logger.LogError(reader.ReadToEnd());
                }
            }
        }
    }
}
