using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardUserController : ControllerBase
    {
        private readonly IUserDashboardService userDashboardService;

        public DashboardUserController(IUserDashboardService userDashboardService)
        {
            this.userDashboardService = userDashboardService;
        }

        //GET: api/dashboardUser?accountId=0&
        [HttpGet()]
        public IActionResult GetAccountByAccountCode([FromQuery] int accountId , [FromQuery] string rangeType)
        {
            try
            {
                if (accountId <= 0) throw new ArgumentException("AccountId is required");
                var result = userDashboardService.GetUserDashboard(accountId, rangeType);
                return StatusCode(StatusCodes.Status200OK, new ResponseHeader
                {
                    Status = "S",
                    Message = "Get user Dashboard success",
                    Content = result
                });
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.InnerException
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }
    }
}
