using System;
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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vproker.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<OrderController> Logger { get; set; }

        private OrderService _service { get; set; }

        public OrderController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, OrderService service)
        {
            _service = service;
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
                orders = AppContext.Orders.Include(o => o.Tool).ToArray();

                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ToolSortParm = sortOrder == "Tool" ? "tool_desc" : "Tool";

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o => o.ClientName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.ClientName).ToArray();
                        break;
                    case "Tool":
                        orders = orders.OrderBy(o => o.Tool.Name).ToArray();
                        break;
                    case "tool_desc":
                        orders = orders.OrderByDescending(o => o.Tool.Name).ToArray();
                        break;
                    default: //name ascending
                        orders = orders.OrderBy(s => s.ClientName).ToArray();
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

                orders = _service.GetActiveOrders(User, sortOrder, searchString).ToArray();
            }

            return View("ActiveOrders", orders);
        }

        public async Task<ActionResult> Details(string id)
        {
            Order order = await _service.GetById(id);

            if (order == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return NotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            //ViewBag.Clients = GetClientsListItems();
            ViewBag.Tools = GetToolsListItems();
            return View();
        }

        //private IEnumerable<SelectListItem> GetClientsListItems(string selectedId = null)
        //{
        //    var tmp = AppContext.Clients.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

        //    // Create authors list for <select> dropdown
        //    return tmp
        //        .OrderBy(client => client.LastName)
        //        .Select(client => new SelectListItem
        //        {
        //            Text = client.FullName,
        //            Value = client.ID.ToString(),
        //            Selected = client.ID == selectedId
        //        });
        //}

        private IEnumerable<SelectListItem> GetToolsListItems(string selectedId = null)
        {
            var tmp = AppContext.Tools.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

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
        public async Task<ActionResult> Create(Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    order.CreatedBy = User.Identity.Name;
                    AppContext.Orders.Add(order);
                    await AppContext.SaveChangesAsync();
                    return RedirectToAction("ActiveOrders");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
            }

            //ViewBag.Clients = GetClientsListItems();
            ViewBag.Tools = GetToolsListItems();
            return View(order);
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

            ViewBag.Tools = GetToolsListItems(order.ToolID);

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
                order.ClientName = newOrder.ClientName;
                order.ClientPhoneNumber = newOrder.ClientPhoneNumber;
                order.Description = newOrder.Description;
                order.PaidPledge = newOrder.PaidPledge;
                order.Price = newOrder.Price;
                order.StartDate = newOrder.StartDate;
                order.EndDate = newOrder.EndDate;
                order.Payment = newOrder.Payment;

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
            return AppContext.Orders.Include(o => o.Tool).SingleOrDefaultAsync(order => order.ID == id);
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
            return View(new CloseOrderModel(order));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(string id, CloseOrderModel model)
        {
            try
            {
                Order order = await FindOrderAsync(id);
                order.Payment = model.TotalPayment;
                order.EndDate = DateTime.UtcNow;

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
            byte[] fileBytes = _service.GetHistoryReport(User, start, end, searchString);

            return File(fileBytes, "text/csv; charset=UTF8", fileName); // this is the key!
        }
    }
}
