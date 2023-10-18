using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
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
    public class FormRequestLockerController : ControllerBase
    {
        private readonly IFormRequestLockerService formRequestLockerService;

        public FormRequestLockerController(IFormRequestLockerService formRequestLockerService)
        {
            this.formRequestLockerService = formRequestLockerService;
        }

        //POST: api/FormRequestLocker
        [HttpPost]
        public IActionResult AddFormRequestLocker([FromBody] FormRequestDto formRequest)
        {
            try
            {
                var result = formRequestLockerService.AddFormRequestLocker(formRequest);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                };
                return StatusCode(StatusCodes.Status400BadRequest, responseHeader);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }

        //GET: api/FormRequestLocker?AccountId=1
        [HttpGet]
        public IActionResult GetFormRequestByAccountId(
            [FromQuery] int accountId,
            [FromQuery] string keyWord,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5
            )
        {
            perPage = perPage == 0 ? int.MaxValue : perPage;
            if (accountId <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader(status: "F", Message: "accountId incorrect"));
            }
            try
            {
                var result = formRequestLockerService.GetAllFormRequestByAccountId(page, perPage, accountId, keyWord);
                return Ok(result);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.InnerException
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }

        //GET: api/FormRequestLocker/filler
        [HttpGet("filler")]
        public IActionResult GetFormRequestByAccountId(
            [FromQuery] int accountId,
            [FromQuery] string FirstName,
            [FromQuery] string LastName,
            [FromQuery] string FormCode,
            [FromQuery] string LockerCode,
            [FromQuery] string LocateName,
            [FromQuery] string SubDistrict,
            [FromQuery] string District,
            [FromQuery] string PostalCode,
            [FromQuery] string Status,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5

            )
        {
            perPage = perPage == 0 ? int.MaxValue : perPage;
            // if (accountId <= 0)
            // {
            //     return StatusCode(StatusCodes.Status400BadRequest, new ResponseHeader(status: "F", Message: "accountId incorrect"));
            // }
            try
            {
                FormRequestDto formRequestDto = new FormRequestDto
                {
                    AccountId = accountId,
                    FirstName = FirstName,
                    LastName = LastName,
                    FormCode = FormCode,
                    LockerCode = LockerCode,
                    LocateName = LocateName,
                    SubDistrict = SubDistrict,
                    District = District,
                    PostalCode = PostalCode,
                    Status = Status
                };
                var result = formRequestLockerService.SearchFormRequest(page, perPage, formRequestDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.InnerException
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }

        //PUT: api/FormRequestLocker/approveform
        [HttpPut("approveform")]
        public IActionResult ApproveForm([FromBody] BaseUpdateDto baseUpdateDto)
        {
            try
            {
                var result = formRequestLockerService.ApproveForm(baseUpdateDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }
        //PUT: api/FormRequestLocker/approveformandinstall
        [HttpPut("approveformandinstall")]
        public IActionResult ApproveFormAndInstall([FromBody] BaseUpdateDto baseUpdateDto)
        {
            try
            {
                var result = formRequestLockerService.ApproveFormAndInstallLocker(baseUpdateDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }


        [HttpPut("createDiagram")]
        public IActionResult CreateDiagram(FormCreateDiagramDto formCreateDiagramDto)
        {
            try
            {
                var result = formRequestLockerService.CreateLockerDiagramByFormId(formCreateDiagramDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                ResponseHeader responseHeader = new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                };
                return StatusCode(StatusCodes.Status500InternalServerError, responseHeader);
            }
        }

    }
}
