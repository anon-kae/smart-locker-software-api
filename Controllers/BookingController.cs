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
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleUser.User)]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        //POST : api/booking
        [HttpPost]
        public IActionResult AddBooking([FromBody]BookingDto bookingDto)
        {
            try
            {
                var result = bookingService.AddBooking(bookingDto);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }

        //GET : api/booking/accountid?accountId=
        [HttpGet("accountid")]
        public IActionResult GetBookingsByAccountId([FromQuery]int accountId)
        {
            try
            {
                var result = bookingService.GetBookings(accountId);
                if (result.Content != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
            
        }

        //GET : api/booking/bookingId?bookingId=
        [HttpGet("bookingId")]
        public IActionResult GetBookingByBookingId([FromQuery] int bookingId)
        {
            try
            {
                var result = bookingService.GetBookingByBookingId(bookingId);
                if (result.Content != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }

        }

        //GET : api/booking/transbookingid?bookingId =
        [HttpGet("transbookingid")]
        public IActionResult GetAccountByResBookingId([FromQuery] int bookingId)
        {
            try
            {
                var result = bookingService.GetAccountByResBookingId(bookingId);
                if (result.Content != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader("F", e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }

        }

        //GET : api/booking/resbookingid?bookingId =
        [HttpGet("resbookingid")]
        public IActionResult GetAccountByTranssBookingId([FromQuery] int bookingId)
        {
            try
            {
                var result = bookingService.GetAccountByTransBookingId(bookingId);
                if (result.Content != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, result);
                }
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader("F", e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }

        }

        //PUT : api/booking
        [HttpPut]
        public IActionResult UpdateBooking([FromBody] BookingUpdateDto bookingUpdateDto)
        {
            try
            {
                var result = bookingService.UpdateBooking(bookingUpdateDto.Status, bookingUpdateDto.BookingId);
                return StatusCode(StatusCodes.Status200OK, result);

               
            }
            catch(ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict,new ResponseHeader("F",e.Message));
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message , e.StackTrace));
            }
        }

        //PUT : api/booking/transfer
        [HttpPut("transfer")]
        public IActionResult RequestTransfer([FromBody] TransferDetailDto transferDetailDto)
        {
            try
            {
               var result = bookingService.BookingRequestTransfer(transferDetailDto.BookingId, transferDetailDto.AccountId);
               return StatusCode(StatusCodes.Status200OK, result);

            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader("F", e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }

        //PUT : api/booking/transfer/cancel
        [HttpPut("transfer/cancel")]
        public IActionResult CancelTransfer([FromBody] TransferDetailDto transferDetailDto)
        {
            try
            {
                var result = bookingService.CancelTransfer(transferDetailDto.BookingId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ConfilctDataException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseHeader("F", e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHeader("F", e.Message, e.StackTrace));
            }
        }
    
        //GET : api/booking/bookvalidate
        
    }
}
