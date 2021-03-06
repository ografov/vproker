﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/client")]
    public class ClientApiController : Controller
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<ClientApiController> Logger { get; set; }

        private ClientService _service { get; set; }

        public ClientApiController(ILoggerFactory loggerFactory, ApplicationDbContext appContext, ClientService service)
        {
            _service = service;
            Logger = loggerFactory.CreateLogger<ClientApiController>();
            AppContext = appContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetAllInfo()
        {
            return Json(_service.GetAllInfo());
        }

        //[HttpGet("{id}", Name = "Get")]
        //public JsonResult Get(string id)
        //{
        //    return Json(_service.GetById(id));
        //}
        
        [HttpGet("getInfoById/{id?}")]
        public async Task<JsonResult> GetInfoById(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            ClientInfo stat = await _service.GetInfoById(User, id);
            return Json(stat);
        }

        [HttpGet("getInfoByPhone/{number?}")]
        public JsonResult GetInfoByPhone(string number)
        {
            if (String.IsNullOrEmpty(number))
                throw new ArgumentNullException(nameof(number));

            var stat = _service.GetInfoByPhone(User, number);
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
        public async Task<ActionResult> Store([FromBody]Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var savedStore = await _service.Store(client);
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
