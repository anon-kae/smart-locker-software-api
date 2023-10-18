using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles =RoleUser.PartnerAndAdmin)]
    public class RepairFormController : ControllerBase
    {
        private readonly IRepairFormService repairFormService;

        public RepairFormController(IRepairFormService repairFormService)
        {
            this.repairFormService = repairFormService;
        }

        // POST: api/repairform
        [HttpPost]
        public IActionResult AddRepairForm([FromBody] RepairFormDto repairFormDto)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, repairFormService.AddRepairForm(repairFormDto));
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
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

        // PUT: api/repairform/approve
        [HttpPut("Approve")]
        public IActionResult ApproveRepairForm([FromBody]RepairFormDto repairFormDto)
        {
            try
            {
                if(!(repairFormDto.Status.Equals("P") || repairFormDto.Status.Equals("R")))
                {
                    throw new ArgumentException("repair form status to update is incorect to this function.");
                }
                 return StatusCode(StatusCodes.Status200OK, repairFormService.ApproveRepairForm(repairFormDto));
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
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

        // PUT: api/repairform/finish
        [HttpPut("Finish")]
        public IActionResult FinishRepairForm([FromBody] RepairFormDto repairFormDto)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, repairFormService.FinishRepairForm(repairFormDto));
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
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
        [HttpGet("summary-repair")]
        public IActionResult GetSummaryRepair(
            [FromQuery]int accountId
            )
        {
            try
            {
                RepairFormDto repairFormDto = new RepairFormDto()
                {
                    AccountId = accountId
                };
                return StatusCode(StatusCodes.Status200OK, repairFormService.GetSummaryRepair(repairFormDto));
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
        // GET: api/repairform
        [HttpGet]
        public IActionResult GetRepairForm(
            [FromQuery]string lockerCode,
            [FromQuery]string firstName,
            [FromQuery]string lastName,
            [FromQuery]string status,
            [FromQuery]int accountId,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5
            )
        {
            try
            {
                RepairFormDto repairFormDto = new RepairFormDto()
                {
                    LockerCode = lockerCode,
                    FirstName = firstName,
                    LastName = lastName,
                    Status = status,
                    AccountId = accountId
                };
                return StatusCode(StatusCodes.Status200OK, repairFormService.GetRepairForm(page,perPage,repairFormDto));
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
        // GET: api/repairform/lockerRoom?lockerId=
        [HttpGet("lockerRoom")]
        public IActionResult GetLockerRoomReady(
            [FromQuery] int? lockerId
            )
        {
            try
            {
                LockerRoomDto lockerRoom = new LockerRoomDto()
                {
                    LockerId = lockerId ?? throw new ArgumentException("Locker Id is required."),   
                };
                return StatusCode(StatusCodes.Status200OK, repairFormService.GetLockerRoomReadyToRepair(lockerRoom));
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
