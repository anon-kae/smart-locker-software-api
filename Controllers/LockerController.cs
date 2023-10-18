using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models;
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
    public class LockerController : ControllerBase
    {
        private readonly ILockerService lockerService;

        public LockerController(ILockerService lockerService)
        {
            this.lockerService = lockerService;
        }

        //GET: api/locker/1
        [HttpGet("{locateId}")]
        public IActionResult GetLockerByLocateId([FromRoute] int locateId)
        {
            try
            {
                var result = lockerService.GetLockerByLocateId(locateId);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET: api/locker/lockerroom/1
        [HttpGet("lockerroom/{lockerid}")]
        public IActionResult GetLockerRoomByLockerId([FromRoute] int lockerid)
        {
            try
            {
                var result = lockerService.GetAllLockerRoomByLockerId(lockerid);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }


        //GET: api/locker/lockerroom?lkroomId
        [HttpGet("lockerroom")]
        public IActionResult GetLockerRoomByLkRoomId([FromQuery] int lkroomID)
        {
            try
            {
                var result = lockerService.GetLockerRoomByLkRoomId(lkroomID);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET: api/locker/validateroom?lkroomId
        [HttpGet("validateroom")]
        public IActionResult ValidateRoom([FromQuery] int lkroomID)
        {
            try
            {
                var result = lockerService.GetLockerRoomByLkRoomId(lkroomID);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader
                    {
                        Status = "F",
                        Message = "no locker room id in database",
                        Content = null
                    });
                }
                else 
                {
                    LockerRoomDto lockerRoomDto = result.Content as LockerRoomDto;
                    if (lockerRoomDto.Status.Equals("A"))
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseHeader
                        {
                            Status = "S",
                            Message = "this lk room is avaliable",
                            Content = null
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader
                        {
                            Status = "F",
                            Message = "locker room id isnot avaliable",
                            Content = null
                        });

                    }
                }

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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }
        //GET: api/locker/lockersize
        [HttpGet("lockersize")]
        public IActionResult GetLockerSize()
        {
            try
            {
                var result = lockerService.GetAllSize();
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }


        //GET: api/locker/servicecharge
        [HttpGet("servicecharge")]
        public IActionResult GetAllServiceCharge()
        {
            try
            {
                var result = lockerService.GetAllServiceCharge();
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET: api/locker/servicecharge/lockerroomid
        [HttpGet("servicecharge/lockerroomid")]
        public IActionResult GetServiceChargeByLockerRoomId([FromQuery] int lkroomId, [FromQuery] int typeId)
        {
            try
            {
                var result = lockerService.GetServiceChargeByLockerRoomId(lkroomId, typeId);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET api/locker/type
        [HttpGet("type")]
        [AllowAnonymous]
        public IActionResult GetAllType()
        {
            try
            {
                var result = lockerService.GetAllType();
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET api/locker/type/typeId?typeid=
        [HttpGet("type/typeid")]
        public IActionResult GetTypeByTypeId([FromQuery] int typeId)
        {
            try
            {
                var result = lockerService.GetTypeByTypeId(typeId);
                if (result.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }
    }
}
