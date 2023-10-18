using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IBookingDA
    {
        Booking AddBooking(Booking booking);
        List<BookingDto> GetBookings(int accountId);
        BookingDto GetBookingByBookingId(int bookingId);
    }
}
