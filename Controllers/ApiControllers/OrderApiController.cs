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

        [HttpPost("createOrder")]
        public JsonResult CreateOrder([FromBody]Order orderBody)
        {
	        var order = new Order()
	        {
		        ClientID = orderBody.Client.ID,
		        Client = orderBody.Client,

		        ToolID = orderBody.Tool?.ID,
		        Tool = orderBody.Tool,

		        ContractNumber = orderBody.ContractNumber,
		        PaidPledge = orderBody.PaidPledge,
		        Description = orderBody.Description,
		        //CreatedBy = user.Identity.Name,

                ClientPassport = orderBody.Client.Passport,
		        ClientName = orderBody.Client.Name,
		        ClientPhoneNumber = orderBody.Client.PhoneNumber
	        };
	        AppContext.Orders.Add(order);
	        return Json("done");
        }

        [HttpGet("actives/{sortOrder?}/{searchString?}")]
        public JsonResult GetActives(string sortOrder = "", string searchString = "")
        {
            var orders = _service.GetActiveOrders(User, sortOrder, searchString);
            return Json(orders);
        }

        [HttpGet("activesNoUser/{sortOrder?}/{searchString?}")]
        public JsonResult GetActivesNoUser(string sortOrder = "", string searchString = "")
        {
	        var orders = _service.GetActiveOrdersNoUser(sortOrder, searchString);
	        return Json(orders);
        }

        [Authorize(Roles = AuthData.ADMIN_ROLE)]
        [HttpGet("history/{start?}/{end?}/{searchString?}")]
        public JsonResult GetHistory(string start = "", string end = "", string searchString = "")
        {
            var orders = _service.GetHistory(User, start, end, searchString);
            return Json(orders);
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
