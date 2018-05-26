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
            Func<Order, bool> search = new Func<Order, bool>((o) =>
            {
                if (String.IsNullOrEmpty(searchString))
                    return true;
                return o.ClientName.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0;
            });

            return AppContext.Orders.Where(o => o.IsClosed && search(o)).Include(o => o.Tool);
        }

        //public IEnumerable<Order> GetHistoryWithTool(ClaimsPrincipal user, string sortOrder, string searchString)
        //{
        //    return AppContext.Orders.Where(o => o.IsClosed).Include(o => o.Tool);
        //}


        public byte[] GetHistoryReport()
        {
            IEnumerable<Order> orders = AppContext.Orders.Where(o => o.IsClosed).Include(o => o.Tool);

            String csv = CreateCSVFromGenericList<Order>(orders);

            //CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            return System.Text.Encoding.UTF8.GetBytes(csv);
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
