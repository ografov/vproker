using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        public IEnumerable<Order> GetActiveOrdersNoUser(string sortOrder, string searchString)
        {
            IEnumerable<Order> orders = Array.Empty<Order>();

            if (!AppContext.Orders.Any()) return orders;
            orders = AppContext.Orders.Include(o => o.Tool).Include(o => o.Client);

            orders = orders.Where(o => !o.IsClosed);

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Client.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.Client.Name).ToArray();
                    break;
                case "tool":
                    orders = orders.OrderBy(o => o.Tool.Name).ToArray();
                    break;
                case "tool_desc":
                    orders = orders.OrderByDescending(o => o.Tool.Name).ToArray();
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.StartDate).ToArray();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.StartDate).ToArray();
                    break;
                default: //date ascending
                    orders = orders.OrderBy(s => s.StartDate).ToArray();
                    break;
            }

            return orders;
        }
        public IEnumerable<Order> GetActiveOrders(ClaimsPrincipal user, string sortOrder, string searchString)
        {
            IEnumerable<Order> orders = new Order[0];

            if (AppContext.Orders.Count() > 0)
            {
                orders = AppContext.Orders.Include(o => o.Tool).Include(o => o.Client);

                orders = orders.Where(o => !o.IsClosed);

                // if not admin, restrict by who created

                if (user.Identity.Name != AuthData.ADMIN_ID)
                {
                    orders = orders.Where(o => o.CreatedBy == user.Identity.Name);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o => o.Client.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.Client.Name).ToArray();
                        break;
                    case "tool":
                        orders = orders.OrderBy(o => o.Tool.Name).ToArray();
                        break;
                    case "tool_desc":
                        orders = orders.OrderByDescending(o => o.Tool.Name).ToArray();
                        break;
                    case "date":
                        orders = orders.OrderBy(o => o.StartDate).ToArray();
                        break;
                    case "date_desc":
                        orders = orders.OrderByDescending(o => o.StartDate).ToArray();
                        break;
                    default: //date ascending
                        orders = orders.OrderBy(s => s.StartDate).ToArray();
                        break;
                }
            }

            return orders;
        }

        public IEnumerable<Order> GetHistory(ClaimsPrincipal user, string start, string end, string searchString)
        {
            DateTime startDate;
            DateTime? starts = null;
            if (DateTime.TryParse(start, out startDate)) starts = startDate;

            DateTime endDate;
            DateTime? ends = null;
            if (DateTime.TryParse(end, out endDate)) ends = endDate;

            // filters
            Func<Order, bool> startFilter = new Func<Order, bool>((o) =>
            {
                return (starts == null) || (o.StartDate >= starts);
            });
            Func<Order, bool> endFilter = new Func<Order, bool>((o) =>
            {
                return (ends == null) || (o.StartDate <= ends);
            });
            Func<Order, bool> clientFilter = new Func<Order, bool>((o) =>
            {
                if (String.IsNullOrEmpty(searchString))
                    return true;
                return o.Client.Name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0;
            });

            return AppContext.Orders.Where(o => o.IsClosed && startFilter(o) && endFilter(o) && clientFilter(o)).Include(o => o.Tool).Include(o => o.Client);
        }

        public Order[] GetTodayClosedOrders(ClaimsPrincipal user)
        {
            return AppContext.Orders.Where(o => o.IsClosed && o.EndDate != null && Equals(o.EndDate.GetValueOrDefault().Date, DateTime.Now.Date)).ToArray();
        }

        public byte[] GetHistoryReport(ClaimsPrincipal user, string start, string end, string searchString)
        {
            var orders = GetHistory(user, start, end, searchString);

            String csv = OrderReport.CreateOrderCSV(orders);

            //CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            var data = System.Text.Encoding.UTF8.GetBytes(csv);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return result;
        }

        public byte[] GetStatsByDaysReport(ClaimsPrincipal user, string start, string end, string searchString)
        {
            var orders = GetHistory(user, start, end, searchString);

            String csv = OrderReport.CreateStatisticsByDays(orders);

            var data = System.Text.Encoding.UTF8.GetBytes(csv);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return result;
        }

        public int SuggestContractNumber()
        {
            var settings = AppContext.Settings.FirstOrDefault();

            var contractNumbers = (settings == null) 
                ? AppContext.Orders.Select(o => o.ContractNumber)
                : AppContext.Orders.Where(o => o.StartDate > settings.StartContractNumberSince).Select(o => o.ContractNumber);

            // if no contracts after settings changed, return the default value
            if (settings != null && !contractNumbers.Any())
            {
                return settings.StartContractNumber;
            }

            // otherwise, give an incremented max number
            return GetMaxNumber(contractNumbers) + 1;
        }

        private static int GetMaxNumber(IEnumerable<string> numberStrings)
        {
            int max = 0;
            foreach (string numStr in numberStrings)
            {
                if (int.TryParse(numStr, out var n) && n > max)
                    max = n;
            }
            return max;
        }
    }
}
