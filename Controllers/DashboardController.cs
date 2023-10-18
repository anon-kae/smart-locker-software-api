using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleUser.PartnerAndAdmin)]
    public class DashboardController : ControllerBase
    {
        private readonly IDashBoardService dashBoardService;

        public DashboardController(IDashBoardService dashBoardService)
        {
            this.dashBoardService = dashBoardService;
        }

        //GET api/dashboard/LocateAndLocation?accountId=14010
        [HttpGet("LocateAndLocation")]
        public IActionResult LocateAndLocation([FromQuery]int accountId)
        {
            try
            {
                var result = dashBoardService.GetLocateAndLockerResultByAccountId(accountId);
                return Ok(result);
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
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
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

        //GET api/dashboard/Income?accountId=16018&rangeGraphType=D
        [HttpGet("Income")]
        public IActionResult IncomeResult(
            [FromQuery] int accountId,
            [FromQuery] string rangeGraphType
            )
        {
            try
            {
                DashboardRequireModel dashboardRequireModel = new DashboardRequireModel
                {
                    AccountId = accountId,
                    RangeGraphType = rangeGraphType
                };
                var result = dashBoardService.IncomeResult(dashboardRequireModel);
                return Ok(result);
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
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
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
        
        //GET api/dashboard/bookingLocker?accountId=16018&rangeGraphType=D
        [HttpGet("bookingLocker")]
        public IActionResult BookingCountFormLocker(
            [FromQuery] int accountId,
            [FromQuery] string rangeGraphType,
            [FromQuery] int? monthRange
            )
        {
            try
            {
                DashboardRequireModel dashboardRequireModel = new DashboardRequireModel
                {
                    AccountId = accountId,
                    RangeGraphType = rangeGraphType,
                    MonthRange = monthRange
                };
                var result = dashBoardService.GetBookingLockerCountByAccountId(dashboardRequireModel);
                return Ok(result);
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
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
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

        //GET api/dashboard/bookingLocate?accountId=16018&rangeGraphType=D
        [HttpGet("bookingLocate")]
        public IActionResult BookingCountFormLocatetest(
            [FromQuery] int accountId,
            [FromQuery] string rangeGraphType,
            [FromQuery] int? monthRange
            )
        {
            try
            {
                DashboardRequireModel dashboardRequireModel = new DashboardRequireModel
                {
                    AccountId = accountId,
                    RangeGraphType = rangeGraphType,
                    MonthRange = monthRange
                };
                var result = dashBoardService.GetBookingLocationCountByAccountId(dashboardRequireModel);
                return Ok(result);
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
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
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
