using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartlocker.software.api.Models;
using smartlocker.software.api.Services.Interfaces;
using SmartLocker.Software.Backend.Models.Output;
//using smartlocker.software.api.Models;

namespace smartlocker.software.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        //POST : api/notification
        [HttpPost]
        public IActionResult AddNotification([FromBody] NotificationDto notificationDto)
        {
            try
            {
                var result = notificationService.AddNotification(notificationDto);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }

        //GET : api/notification/eventtype
        [HttpGet("eventtype")]
        [AllowAnonymous]
        public IActionResult GetAllEventType()
        {
            try
            {
                var result = notificationService.GetAllEventType();
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }

        //GET : api/notification?page=1&perpage=20&accountId=4
        [HttpGet]
        public IActionResult GetAllNotificationByAccountId([FromQuery]int page = 1 , [FromQuery] int perPage = 20, [FromQuery]int accountId = 0)
        {
            try
            {
                if(accountId == 0)
                {
                    throw new ArgumentException("accountId is Required.");
                }
                var result = notificationService.GetAllNotificationByAccountId(page , perPage,accountId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch(ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader("F", e.Message, e.StackTrace));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }

    }
}