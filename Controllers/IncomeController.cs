using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models;
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
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService incomeService;
        private readonly string url = "https://webservice-locker.ml/report/";
        public IncomeController(IIncomeService incomeService)
        {
            this.incomeService = incomeService;
        }

        // GET api/income/location?AccountId=
        [HttpGet("location")]
        public ActionResult GetLocationIncome(
            [FromQuery] string LocateName,
            [FromQuery] string FirstName,
            [FromQuery] string LastName,
            [FromQuery] string SubDistrict,
            [FromQuery] string District,
            [FromQuery] string Province,
            [FromQuery] string PostalCode,
            [FromQuery] string Status,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5)
        {
            try
            {
                LocationDto keyword = new LocationDto
                {
                    LocateName = LocateName,
                    FirstName = FirstName,
                    LastName = LastName,
                    SubDistrict = SubDistrict,
                    District = District,
                    Province = Province,
                    PostalCode = PostalCode,
                    Status = "A",
                    AccountId = AccountId == null ? throw new ArgumentException("AccountId is required") : AccountId.Value
                };
                IncomeDto income = new IncomeDto
                {
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    LocationDto = keyword
                };
                var result = incomeService.GetIncomeLocation(page, perPage, income);
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

        // GET api/income/location/excel?AccountId=
        [HttpGet("location/excel")]
        public ActionResult GetLocationIncomeExcel(
            [FromQuery] string LocateName,
            [FromQuery] string FirstName,
            [FromQuery] string LastName,
            [FromQuery] string SubDistrict,
            [FromQuery] string District,
            [FromQuery] string Province,
            [FromQuery] string PostalCode,
            [FromQuery] string Status,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = Int32.MaxValue)
        {
            try
            {
                LocationDto keyword = new LocationDto
                {
                    LocateName = LocateName,
                    FirstName = FirstName,
                    LastName = LastName,
                    SubDistrict = SubDistrict,
                    District = District,
                    Province = Province,
                    PostalCode = PostalCode,
                    Status = "A",
                    AccountId = AccountId == null ? throw new ArgumentException("AccountId is required") : AccountId.Value
                };
                IncomeDto income = new IncomeDto
                {
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    LocationDto = keyword
                };
                var filename = incomeService.GetIncomeLocationExcel(page, perPage, income);
                return Ok(url + filename);
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

        // GET api/income/locker?AccountId=
        [HttpGet("locker")]
        public ActionResult GetLockerIncome(
            [FromQuery] string LockerCode,
            [FromQuery] int LocateId,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5)
        {
            try
            {
                LockerDto locker = new LockerDto
                {
                    LocateId = LocateId,
                    LockerCode = LockerCode,
                    AccountId = AccountId
                };
                IncomeLockerDto income = new IncomeLockerDto
                {
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    LockerFilter = locker
                };
                var result = incomeService.GetIncomeLocker(page, perPage, income);
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

        // GET api/income/locker/excel?AccountId=
        [HttpGet("locker/excel")]
        public ActionResult GetLockerIncomeExcel(
            [FromQuery] string LockerCode,
            [FromQuery] int LocateId,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = Int32.MaxValue)
        {
            try
            {
                LockerDto locker = new LockerDto
                {
                    LocateId = LocateId,
                    LockerCode = LockerCode,
                    AccountId = AccountId
                };
                IncomeLockerDto income = new IncomeLockerDto
                {
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    LockerFilter = locker
                };
                var filename = incomeService.GetIncomeLockerExcel(page, perPage, income);
                return Ok(url + filename);
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
                    Content = e.InnerException
                });
            }
        }

        // GET api/income/lockerroom?AccountId=
        [HttpGet("lockerroom")]
        public ActionResult GetLockerRoomIncome(
            [FromQuery] int LockerId,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null)
        {
            try
            {

                IncomeLockerRoomDto income = new IncomeLockerRoomDto
                {
                    LockerId = LockerId,
                    AccountId = AccountId,
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };
                var result = incomeService.GetIncomeLockerRoom(income);
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


        // GET api/income/lockerroom/excel?AccountId=
        [HttpGet("lockerroom/excel")]
        public ActionResult GetLockerRoomIncomeExcel(
            [FromQuery] int LockerId,
            [FromQuery] int? AccountId,
            [FromQuery] string DurationType,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null)
        {
            try
            {
                IncomeLockerRoomDto income = new IncomeLockerRoomDto
                {
                    LockerId = LockerId,
                    AccountId = AccountId,
                    DurationType = DurationType,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };
                var filename = incomeService.GetIncomeLockerRoomExcel(income);
                return Ok(url+filename);
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
                    Content = e.InnerException
                });
            }
        }
    }
}
