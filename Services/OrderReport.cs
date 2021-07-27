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
                string[] items = { item.Client?.Name, item.Client?.PhoneNumber ?? "", item.Description ?? "", item.StartDate.ToShortDateString(), item.EndDate?.ToShortDateString() ?? "", item.Payment.GetValueOrDefault().ToString("C"), item.CreatedBy ?? "" };
                string line = String.Join(',', items.Select(i => i.Replace(',', ' ')).ToArray());
                sw.Append(line);
                sw.Append(newLine);
            }

            sw.Append(newLine + newLine);

            sw.Append($"Всего: {orders.Sum(o => o.Payment.GetValueOrDefault()).ToString("C")}");

            return sw.ToString();
        }


        public static string CreateStatisticsByDays(IEnumerable<Order> orders)
        {
            if (orders == null || orders.Count() == 0) return "";

            string newLine = Environment.NewLine;

            StringBuilder sw = new StringBuilder();

            //this is the header row
            sw.Append(",Понедельник,Вторник,Среда,Четверг,Пятница,Суббота,Воскресенье");
            sw.Append(newLine);

            DateTime minDate = DateTime.MaxValue;
            Dictionary<int, int> startedByDays = new Dictionary<int, int>();
            Dictionary<int, int> closedByDays = new Dictionary<int, int>();

            foreach (var order in orders)
            {
                DateTime startDate = order.StartDate.ToRussianTime();
                if(startDate < minDate)
                {
                    minDate = startDate;
                }

                int startKey = (startDate.DayOfWeek == DayOfWeek.Sunday) ? 7 : (int)startDate.DayOfWeek;
                if (startedByDays.ContainsKey(startKey))
                {
                    startedByDays[startKey] += 1;
                }
                else
                {
                    startedByDays[startKey] = 1;
                }

                if (order.EndDate.HasValue)
                {
                    DateTime endDate = order.EndDate.Value.ToRussianTime();
                    int endKey = (endDate.DayOfWeek == DayOfWeek.Sunday) ? 7 : (int)endDate.DayOfWeek;
                    if (closedByDays.ContainsKey(endKey))
                    {
                        closedByDays[endKey] += 1;
                    }
                    else
                    {
                        closedByDays[endKey] = 1;
                    }
                }
            }

            sw.AppendLine("Начало Аренды, " + String.Join(',', startedByDays.OrderBy(kv => kv.Key).Select(kv => kv.Value)));
            sw.AppendLine("Конец Аренды, " + String.Join(',', closedByDays.OrderBy(kv => kv.Key).Select(kv => kv.Value)));

            sw.AppendLine();
            sw.Append($"Всего {orders.Count()} начиная с даты: {minDate.ToLongDateString()}");
            return sw.ToString();
        }

    }
}
