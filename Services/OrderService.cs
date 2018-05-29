using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public IEnumerable<Order> GetOrdersByPhoneNumber(ClaimsPrincipal user, string phoneNumber, bool onlyClosed = false)
        {
            return AppContext.Orders.Where(o => String.Equals(o.ClientPhoneNumber, phoneNumber, StringComparison.InvariantCultureIgnoreCase) && (onlyClosed ? o.IsClosed : true));
        }

        public ClientOrderStat GetByPhoneInfo(ClaimsPrincipal user, string phoneNumber)
        {
            var orders = GetOrdersByPhoneNumber(user, phoneNumber);

            return new ClientOrderStat { All = orders.Count(), Active = orders.Where(o => !o.IsClosed).Count() };
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
                return o.ClientName.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0;
            });

            return AppContext.Orders.Where(o => o.IsClosed && startFilter(o) && endFilter(o) && clientFilter(o)).Include(o => o.Tool);
        }

        public byte[] GetHistoryReport(ClaimsPrincipal user, string start, string end, string searchString)
        {
            var orders = GetHistory(user, start, end, searchString);

            String csv = CreateOrderCSV(orders);

            //CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            var data = System.Text.Encoding.UTF8.GetBytes(csv);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return result;
        }

        private string CreateOrderCSV(IEnumerable<Order> orders)
        {
            if (orders == null || orders.Count() == 0) return "";

            string newLine = Environment.NewLine;

            StringBuilder sw = new StringBuilder();

            //this is the header row
            sw.Append("Клиент,Телефон,Описание,Начало,Конец,Сумма,Кем");
            sw.Append(newLine);

            //this acts as datarow
            foreach (Order item in orders)
            {
                string[] items = { item.ClientName, item.ClientPhoneNumber ?? "", item.Description ?? "", item.StartDate.ToShortDateString(), item.EndDate?.ToShortDateString() ?? "", item.PaidPledge.ToString(), item.CreatedBy ?? "" };
                string line = String.Join(',', items.Select(i => i.Replace(',', ' ')).ToArray());
                sw.Append(line);
                sw.Append(newLine);
            }

            sw.Append(newLine + newLine);

            sw.Append($"Всего: {orders.Sum(o => o.PaidPledge)}");

            return sw.ToString();
        }


        /// <summary>
        /// Creates the CSV from a generic list.
        /// </summary>;
        /// <typeparam name="T"></typeparam>;
        /// <param name="list">The list.</param>;
        /// <param name="csvNameWithExt">Name of CSV (w/ path) w/ file ext.</param>;
        public static string CreateCSVFromGenericList<T>(IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0) return "";

            //get type from 0th member
            Type t = list.FirstOrDefault().GetType();
            string newLine = Environment.NewLine;

            StringBuilder sw = new StringBuilder();

            //make a new instance of the class name we figured out to get its props
            object o = Activator.CreateInstance(t);
            //gets all properties
            PropertyInfo[] props = o.GetType().GetProperties();

            //foreach of the properties in class above, write out properties
            //this is the header row
            foreach (PropertyInfo pi in props)
            {
                sw.Append(pi.Name.ToUpper() + ",");
            }
            sw.Append(newLine);

            //this acts as datarow
            foreach (T item in list)
            {
                //this acts as datacolumn
                foreach (PropertyInfo pi in props)
                {
                    //this is the row+col intersection (the value)
                    string whatToWrite =
                        Convert.ToString(item.GetType()
                                             .GetProperty(pi.Name)
                                             .GetValue(item, null))
                            .Replace(',', ' ') + ',';

                    sw.Append(whatToWrite);

                }
                sw.Append(newLine);
            }

            return sw.ToString();
        }
    }
}
