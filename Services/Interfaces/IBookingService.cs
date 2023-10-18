using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IBookingService 
    {
        ResponseHeader AddBooking(BookingDto booking);
        ResponseHeader GetBookings(int accountId);
        ResponseHeader GetBookingByBookingId(int bookingId);
        ResponseHeader UpdateBooking(string status , int bookingId);
        ResponseHeader BookingRequestTransfer(int bookingId, int accountReceiveId);
        ResponseHeader GetAccountByResBookingId(int transBookingId);
        ResponseHeader GetAccountByTransBookingId(int resBookingId);
        ResponseHeader CancelTransfer(int transBookingId);

    }
}
