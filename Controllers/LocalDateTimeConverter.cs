//using Microsoft.AspNet.Mvc;
//using Microsoft.AspNet.Mvc.Filters;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace vproker.Controllers
//{
//    public class LocalDateTimeConverter : ActionFilterAttribute
//    {
//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            Controller controller = filterContext.Controller as Controller;
//            var model = controller.ViewData.Model;
//            if (model != null)// && filterContext.HttpContext.Session["LocalTimeZoneOffset"] != null)
//                ProcessDateTimeProperties(model, filterContext);
//            base.OnActionExecuted(filterContext);
//        }

//        private void ProcessDateTimeProperties(object obj, ActionExecutedContext filterContext)
//        {
//            if (obj.GetType().IsGenericType)
//            {
//                foreach (var item in (IList)obj)
//                {
//                    ProcessDateTimeProperties(item, filterContext);
//                }
//            }
//            else
//            {
//                TypeAccessor member;
//                List<PropertyInfo> props = new List<PropertyInfo>();
//                props.AddRange(obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).ToList());
//                member = TypeAccessor.Create(obj.GetType());
//                foreach (PropertyInfo propertyInfo in props)
//                {
//                    if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
//                    {
//                        {
//                            member[obj, propertyInfo.Name] = DateTimeConverter.ToLocalDatetime((DateTime?)propertyInfo.GetValue(obj)); //, ((int)filterContext.HttpContext.Session["LocalTimeZoneOffset"]));
//                        }
//                    }
//                    else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.GetValue(obj) != null)
//                    {
//                        foreach (var item in (IList)propertyInfo.GetValue(obj))
//                        {
//                            ProcessDateTimeProperties(item, filterContext);
//                        }
//                    }
//                }
//            }
//        }
//    }

//    public class DateTimeConverter
//    {
//        public static DateTime? ToLocalDatetime(DateTime? serverDate, int offset = 3)
//        {
//            if (serverDate == null) return null;
//            return serverDate.Value.AddHours(offset * -1);
//        }

//    }
//}
