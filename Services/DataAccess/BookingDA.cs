using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class BookingDA : IBookingDA
    {
        private readonly IBaseRepository baseRepository;

        public BookingDA(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public Booking AddBooking(Booking booking)
        {
            using var context = new SmartLockerContext();
            
            var result =  context.Booking.Add(booking).Entity;
            context.SaveChanges();
            var lkRoomUpdate = context.LockerRooms.Where(c => c.LkRoomId == booking.LkRoomId).FirstOrDefault();
            lkRoomUpdate.Status = "B";
            lkRoomUpdate.UpdateDate = DateTime.Now;
            context.SaveChanges();
            return result;
        }

        public List<BookingDto> GetBookings(int accountId)
        {
            string sql = @$"SELECT B.[BookingId]
                                  ,B.[OverTime]
                                  ,B.[LkRoomId]
                                  ,B.[AccountId]
                                  ,B.[Status]
                                  ,B.[StartDate]
                                  ,B.[EndDate]
                                  ,B.[CreateDate]
                                  ,B.[UpdateDate]
                                  ,B.[PassCode]
                                    ,B.[RateTypeId]
                                  ,LO.[LocateName]
                                  ,LR.[LkRoomCode]
                              FROM [dbo].[BOOKING] B
                                INNER JOIN LOCKER_ROOMS LR ON B.[LkRoomId] = LR.[LkRoomId] AND LR.Status <> 'D'
                                INNER JOIN LOCKERS LK ON  LK.[LockerId] = LR.[LockerId]  AND LK.Status = 'A'
                                INNER JOIN LOCATIONS LO  ON LO.[LocateId] = LK.[LocateId]  AND LO.Status = 'A'
							  WHERE B.Status <> 'D'  AND B.[AccountId] = {accountId}
							  ORDER BY CreateDate DESC  ";
            var result = baseRepository.QueryString<BookingDto>(sql).ToList();
            return result;
        }

        public BookingDto GetBookingByBookingId(int bookingId)
        {
            string sql = @$"SELECT B.[BookingId]
                                  ,B.[OverTime]
                                  ,B.[LkRoomId]
                                  ,B.[AccountId]
                                  ,B.[Status]
                                  ,B.[StartDate]
                                  ,B.[EndDate]
                                  ,B.[CreateDate]
                                  ,B.[UpdateDate]
                                  ,B.[PassCode]
                                  ,LO.[LocateName]
                                  ,LR.[LkRoomCode]
                              FROM [dbo].[BOOKING] B
                                INNER JOIN LOCKER_ROOMS LR ON B.[LkRoomId] = LR.[LkRoomId] AND LR.Status <> 'D'
                                INNER JOIN LOCKERS LK ON  LK.[LockerId] = LR.[LockerId]  AND LK.Status = 'A'
                                INNER JOIN LOCATIONS LO  ON LO.[LocateId] = LK.[LocateId]  AND LO.Status = 'A'
							  WHERE B.Status <> 'D'  AND B.[BookingId] = {bookingId}
							  ORDER BY CreateDate DESC  ";
            var result = baseRepository.QueryString<BookingDto>(sql).FirstOrDefault();
            return result;
        }


    }
}
