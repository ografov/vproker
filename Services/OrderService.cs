using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public class OrderService
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<OrderService> Logger { get; set; }

        public OrderService(ILoggerFactory loggerFactory, ApplicationDbContext appContext)
        {
            Logger = loggerFactory.CreateLogger<OrderService>();
            AppContext = appContext;
        }

        public async Task<Order> Store(Order order)
        {
            if (AppContext.Orders.Any(e => e.ID == order.ID))
            {
                AppContext.Orders.Attach(order);
                AppContext.Entry(order).State = EntityState.Modified;
            }
            else
            {
                AppContext.Orders.Add(order);
            }

            AppContext.SaveChanges();

            return await GetById(order.ID);
        }

        public async Task<Order> GetById(string id)
        {
            Order order = await AppContext.Orders.Include(b => b.Tool).SingleOrDefaultAsync(b => b.ID == id);

            return order;
        }

        public IEnumerable<Order> GetActiveOrders(ClaimsPrincipal user, string sortOrder, string searchString)
        {
            IEnumerable<Order> orders = new Order[0];

            if (AppContext.Orders.Count() > 0)
            {
                orders = AppContext.Orders.Include(o => o.Tool);

                orders = orders.Where(o => !o.IsClosed);

                // if not admin, restrict by who created
                if (user.Identity.Name != AuthData.ADMIN_ID)
                {
                    orders = orders.Where(o => o.CreatedBy == user.Identity.Name);
                }

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
            }

            return orders;
        }


        public IEnumerable<Order> GetHistory(ClaimsPrincipal user, string sortOrder, string searchString)
        {
            return AppContext.Orders.Where(o => o.IsClosed);
        }

        public IEnumerable<Order> GetHistoryWithTool(ClaimsPrincipal user, string sortOrder, string searchString)
        {
            return AppContext.Orders.Where(o => o.IsClosed).Include(o => o.Tool);
        }
    }
}
