using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("connect")]
        [AllowAnonymous]
        public IActionResult testAccount()
        {

            return Ok("connected");
        }

        //POST: api/account/login/manager
        [HttpPost("login/manager")]
        [AllowAnonymous]
        public IActionResult LoginAccountManager([FromBody] FormLoginDto formLoginDto)
        {
            try
            {
                var checkPattern = formLoginDto.Email.Split('@')[0];
                if (checkPattern != "admin")
                    formLoginDto.RoleUser ??= RoleUser.Partner;
                else
                    formLoginDto.RoleUser ??= RoleUser.Admin;
                return StatusCode(StatusCodes.Status200OK, accountService.LoginAccount(formLoginDto));
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
            catch (UnauthorizedAccessException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseHeader
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
        //POST: api/account/login
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult LoginAccount([FromBody] FormLoginDto formLoginDto)
        {
            try
            {
                formLoginDto.RoleUser ??= RoleUser.User;
                Console.WriteLine(formLoginDto.RoleUser);
                return StatusCode(StatusCodes.Status200OK, accountService.LoginAccount(formLoginDto));
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
            catch (UnauthorizedAccessException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseHeader
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

        //POST: api/account/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAccount([FromBody] FormRegisterDto formRegister)
        {
            try
            {
                var data = await accountService.RegisterAccountAsync(formRegister);
                if (data.Status == "F")
                    return StatusCode(StatusCodes.Status400BadRequest, data);
                else
                    return StatusCode(StatusCodes.Status200OK, data);
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

        //GET: api/account/email?email=""
        [HttpGet("email")]
        [AllowAnonymous]
        public IActionResult ValidateDuplicateEmail([FromQuery] string email)
        {
            try
            {
                var emailResult = accountService.ValidateDuplicateEmail(email);
                if (emailResult.Content == null)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, emailResult);
                }
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

        [HttpGet("emailpartner")]
        [AllowAnonymous]
        public IActionResult ValidateDuplicateEmailPartner([FromQuery] string email)
        {
            try
            {
                var emailResult = accountService.ValidateDuplicateEmailPartner(email);
                if (emailResult.Content == null)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, emailResult);
                }
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
        //GET: api/account/accountcode?accountcode=""
        [HttpGet("accountcode")]
        public IActionResult GetAccountByAccountCode([FromQuery] string accountcode)
        {
            try
            {
                var emailResult = accountService.GetAccountByAccountCode(accountcode);
                if (emailResult.Content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, null);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, emailResult);
                }
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

        //PUT: api/account
        //update first and lastname only
        [HttpPut]
        public IActionResult UpdateAccount([FromBody] AccountDto accountDto)
        {
            try
            {
                var emailResult = accountService.UpdateAccount(accountDto);
                return StatusCode(StatusCodes.Status200OK, emailResult);
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

        //GET: api/account/partner?firstName=
        [HttpGet("partner")]
        [Authorize(Roles = RoleUser.Admin)]
        public IActionResult GetPartnerAccount(

            [FromQuery] string firstName,
            [FromQuery] string lastName,
            [FromQuery] string accountCode,
            [FromQuery] string status,
            [FromQuery] string email,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 5)
        {
            try
            {
                AccountDto account = new AccountDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    AccountCode = accountCode,
                    Status = status,
                    Email = email
                };

                var result = accountService.GetPartnerAccount(page, perPage, account);
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

        // [AllowAnonymous]
        // [HttpGet("test")]
        // public IActionResult Index()
        // {
        //     return new ContentResult
        //     {
        //         ContentType = "text/html",
        //         StatusCode = (int)HttpStatusCode.OK,
        //         Content = "<html><body>Hello World</body></html>"
        //     };
        // }
        [AllowAnonymous]
        [HttpGet("VerifyAccount")]
        // [ValidateAntiForgeryToken]
        public IActionResult ApproveAccount([FromQuery] string email, [FromQuery] int id)
        {
            try
            {
                var verifyAccount = accountService.VerifyAccount(email, id);
                if (verifyAccount.Status.Equals("F"))
                {
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        StatusCode = (int)HttpStatusCode.OK,
                        Content = "<html><body><div align='center'>This account has been verified </div></body></html>"
                    };
                }
                else
                {
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        StatusCode = (int)HttpStatusCode.OK,
                        Content = "<html><body><div align='center'>Verify Success</div></body></html>"
                    };
                }
                // return StatusCode(StatusCodes.Status200OK, verifyAccount);
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



        //PUT: api/account/approve
        //Send only id and status
        [Authorize(Roles = RoleUser.Admin)]
        [HttpPut("approve")]
        public IActionResult ApproveAccount([FromBody] AccountDto accountDto)
        {
            try
            {
                var approveResult = accountService.ApproveAccount(accountDto);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }




        //[HttpGet("user")]
        //[Authorize(Roles = RoleUser.User)]
        //public string Testuser()
        //{
        //    return "user get data successful";
        //}

        //[HttpGet("partner")]
        //[Authorize(Roles = RoleUser.Partner)]
        //public string Testpartner()
        //{
        //    return "partner get data successful";
        //}


        //[HttpGet("admin")]
        //[Authorize(Roles = RoleUser.Admin)]
        //public string Testadmin()
        //{
        //    return "admin get data successful";
        //}
    }
}
