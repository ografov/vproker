using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vproker.Models;
using vproker.Services;

namespace vproker.Controllers.ApiControllers
{
    [AllowAnonymous]
    [Route("api/order")]
    public class OrderApiController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<OrderApiController> Logger { get; set; }

        private OrderService _service { get; set; }

        public OrderApiController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, OrderService service)
        {
            _service = service;
            Logger = loggerFactory.CreateLogger<OrderApiController>();
            AppContext = appContext;
        }

        [HttpGet("actives/{sortOrder?}/{searchString?}")]
        public JsonResult GetActives(string sortOrder = "", string searchString = "")
        {
            var orders = _service.GetActiveOrders(User, sortOrder, searchString);
            return Json(orders);
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        [HttpGet("history/{start?}/{end?}/{searchString?}")]
        public JsonResult GetHistory(string start = "", string end = "", string searchString = "")
        {
            var orders = _service.GetHistory(User, start, end, searchString);
            return Json(orders);
        }

        [HttpGet("getByPhoneInfo/{number?}")]
        public JsonResult GetByPhoneInfo(string number)
        {
            if (String.IsNullOrEmpty(number))
                throw new ArgumentNullException(nameof(number));

            var stat = _service.GetClientInfo(User, number);
            return Json(stat);
        }

        [HttpGet("validatePassport/{passport?}")]
        public JsonResult ValidatePassport(string passport)
        {
            if (String.IsNullOrEmpty(passport))
                throw new ArgumentNullException(nameof(passport));

            try
            {
                var stat = _service.ValidatePassport(User, passport);
                return Json(stat);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Store([FromBody]Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var savedStore = await _service.Store(order);
                    return Json(savedStore);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения: " + ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
