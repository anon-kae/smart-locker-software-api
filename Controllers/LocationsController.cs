using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartlocker.software.api.Models;
using smartlocker.software.api.Services.Interfaces;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
//using smartlocker.software.api.Models;

namespace smartlocker.software.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService locationService;
        public LocationsController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        // GET api/location
        [HttpGet()]
        public ActionResult GetLocation()
        {
            try
            {
                var locationResult = locationService.GetLocation();
                if (locationResult.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, locationResult);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, locationResult);
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

        // GET api/locations/5
        [HttpGet("{id}")]
        public ActionResult GetLocationById(int id)
        {
            try
            {
                var data = locationService.GetLocationById(id);
                
                    return StatusCode(StatusCodes.Status200OK, data);

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

        [Authorize(Roles = RoleUser.Admin)]
        [HttpPut("approve")]
        public ActionResult ApproveLocation([FromBody] LocationDto location)
        {
            try
            {

                var approveResult = locationService.ApproveLocation(location);
                return StatusCode(StatusCodes.Status200OK, approveResult);

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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }
        // GET api/locations/all
        [HttpGet("all")]
        [Authorize(Roles = RoleUser.Admin)]
        public ActionResult GetLocationAll(
                   [FromQuery] int page = 1,
                   [FromQuery] int perPage = 10)
        {
            try
            {
                GetAmountDataDto getAmount = new GetAmountDataDto
                {
                    Page = page,
                    PerPage = perPage == 0 ? int.MaxValue : perPage
                };
                var result = locationService.GetLocationAll(getAmount);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader("F", e.Message, e.StackTrace));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }
        // GET api/locations/accountId/{accountId}
        [HttpGet("accountId/{accountId}")]
        public ActionResult GetLocationByAccountId(
            [FromRoute] int accountId,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 10)
        {
            try
            {
                if (perPage == 0)
                {
                    perPage = int.MaxValue;
                }
                var data = locationService.GetLocationByAccountId(page, perPage, accountId);
                List<LocationDto> list = data.Content as List<LocationDto>;
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, data);
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
        // GET api/locations/search?keywords=
        [HttpGet("search")]
        public ActionResult SearchLocation([FromQuery] string keywords)
        {
            if (string.IsNullOrEmpty(keywords)) return StatusCode(StatusCodes.Status400BadRequest, "no keywords");
            try
            {
                var data = locationService.SearchLocation(keywords);
                if (data.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, data);
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

        // GET api/locations/filter?page=
        [HttpGet("filter")]
        public ActionResult SearchLocationByLocationDto(
            [FromQuery] string LocateName,
            [FromQuery] string FirstName,
            [FromQuery] string LastName,
            [FromQuery] string SubDistrict,
            [FromQuery] string District,
            [FromQuery] string Province,
            [FromQuery] string PostalCode,
            [FromQuery] string Status,
            [FromQuery] int AccountId,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5
            )
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
                    Status = Status,
                    AccountId = AccountId
                };
                var data = locationService.SearchLocationFormLocationDto(page,perPage,keyword);
                if (data.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, data);
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
        //POST api/locations
        [HttpPost]
        public IActionResult AddLocation([FromBody] LocationDto locationDto)
        {
            try
            {
                var data = locationService.AddLocation(locationDto);
                return StatusCode(StatusCodes.Status200OK, data);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader()
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.InnerException
                });
            }
        }


    }
}