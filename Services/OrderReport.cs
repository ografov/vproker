using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public class OrderReport
    {
        public static string CreateOrderCSV(IEnumerable<Order> orders)
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
                string[] items = { item.Client?.Name, item.Client?.PhoneNumber ?? "", item.Description ?? "", item.StartDate.ToShortDateString(), item.EndDate?.ToShortDateString() ?? "", item.Payment.ToString(), item.CreatedBy ?? "" };
                string line = String.Join(',', items.Select(i => i.Replace(',', ' ')).ToArray());
                sw.Append(line);
                sw.Append(newLine);
            }

            sw.Append(newLine + newLine);

            sw.Append($"Всего: {orders.Sum(o => o.Payment)}");

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
