using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace vproker.Controllers
{
    //public class LocalDateTimeConverter : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        Controller controller = filterContext.Controller as Controller;
    //        var model = controller.ViewData.Model;
    //        if (model != null)// && filterContext.HttpContext.Session["LocalTimeZoneOffset"] != null)
    //            ProcessDateTimeProperties(model, filterContext);
    //        base.OnActionExecuted(filterContext);
    //    }

    //    private void ProcessDateTimeProperties(object obj, ActionExecutedContext filterContext)
    //    {
    //        if (obj.GetType().IsGenericType)
    //        {
    //            foreach (var item in (IList)obj)
    //            {
    //                ProcessDateTimeProperties(item, filterContext);
    //            }
    //        }
    //        else
    //        {
    //            TypeAccessor member;
    //            List<PropertyInfo> props = new List<PropertyInfo>();
    //            props.AddRange(obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).ToList());
    //            member = TypeAccessor.Create(obj.GetType());
    //            foreach (PropertyInfo propertyInfo in props)
    //            {
    //                if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
    //                {
    //                    if(obj.t)
    //                    {
    //                       var localDateTime = DateTimeConverter.ToLocalDatetime((DateTime?)propertyInfo.GetValue(obj));
    //                    }
    //                }
    //                else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.GetValue(obj) != null)
    //                {
    //                    foreach (var item in (IList)propertyInfo.GetValue(obj))
    //                    {
    //                        ProcessDateTimeProperties(item, filterContext);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public static class DateTimeConverter
    {
        const int TIME_ZONE_OFFSET = 3;
        public static DateTime ToLocalDatetime(DateTime serverDate)
        {
            return serverDate.AddHours(TIME_ZONE_OFFSET);
        }


        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Moscow Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }
    }


}
