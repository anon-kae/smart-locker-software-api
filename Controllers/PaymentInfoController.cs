using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omise;
using Omise.Models;
using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentInfoController : ControllerBase
    {
        private readonly IPaymentInfoService paymentInfoService;
        private readonly Client client;
        private readonly string PublicKey = "pkey_test_5lk9eu74kzb3ggevga2";
        private readonly string Secretkey = "skey_test_5lk9eu74uyzpfy8rd4q";
        public PaymentInfoController(IPaymentInfoService paymentInfoService)
        {
            this.paymentInfoService = paymentInfoService;
            this.client = new Client(this.PublicKey, this.Secretkey);
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentInfoDto model)
        {
            try
            {
                // var data = await paymentInfoService.AddPaymentTransaction(model);
                // return Ok(data);
                var spiltDate = model.ExpDate.ToShortDateString().Split('/');
                int ExpirationMonth = Convert.ToInt32(spiltDate[0]);
                int ExpirationYear = Convert.ToInt32(spiltDate[2]);
                var amount = model.Amount.ToString();
                var newAmount = amount + "00";
                var token = await client.Tokens.Create(new CreateTokenRequest
                {
                    Name = model.CardName,
                    Number = model.CardNumber,
                    SecurityCode = model.CVV,
                    ExpirationMonth = ExpirationMonth,
                    ExpirationYear = ExpirationYear
                });
                var charge = await client.Charges.Create(new CreateChargeRequest
                {
                    Amount = Convert.ToInt32(newAmount),
                    Currency = "THB",
                    Card = token.Id
                });
                return StatusCode(StatusCodes.Status200OK, new ResponseHeader()
                {
                    Status = "S",
                    Message = "Payment success",
                    Content = charge.Transaction
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
        //GET : api/paymentinfo/{accountid}
        [HttpGet("{accountid}")]
        public IActionResult GetAllPaymentInfoByAccountId(int accountid)
        {
            try
            {
                var result = paymentInfoService.GetPaymentInfoByAccountId(accountid);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        //GET : api/paymentinfo/cardtype
        [HttpGet("cardtype")]
        public IActionResult GetAllCardType()
        {
            try
            {
                var result = paymentInfoService.GetAllCardType();
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader
                {
                    Status = "F",
                    Message = e.Message,
                    Content = e.StackTrace
                });
            }
        }

        [HttpPost]
        public IActionResult AddPaymentInfo([FromBody] PaymentInfoDto paymentInfoDto)
        {
            //return StatusCode(StatusCodes.Status200OK,null);
            Console.WriteLine("inside services");
            try
            {
                var result = paymentInfoService.InsertPaymentInfo(paymentInfoDto);
                return StatusCode(StatusCodes.Status201Created, result);
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
