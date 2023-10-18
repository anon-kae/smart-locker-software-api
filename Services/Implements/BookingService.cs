using AutoMapper;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class BookingService : IBookingService
    {
        private readonly IMapper mapper;
        private readonly IBookingDA bookingDA;

        public BookingService(IBookingDA bookingDA, IMapper mapper)
        {
            this.bookingDA = bookingDA;
            this.mapper = mapper;
        }
        public ResponseHeader AddBooking(BookingDto booking)
        {
            Random random = new Random();

            Booking bookingEntity = new Booking
            {
                OverTime = 0,
                LkRoomId = booking.LkRoomId,
                AccountId = booking.AccountId,
                Status = "WP",
                CreateDate = DateTime.Now,
                PassCode = random.Next(100000, 999999).ToString(),
            //PassCode = EncryptionUtilities.CreatePasswordSalt(random.Next(100000, 999999).ToString()),
                RateTypeId = booking.RateTypeId
        };
            var result = bookingDA.AddBooking(bookingEntity);
            return new ResponseHeader("S", "Add Booking sucessful", result);
        }
        public ResponseHeader GetBookings(int accountId)
        {
            var result = bookingDA.GetBookings(accountId);
            return new ResponseHeader("S", "Get Bookings sucessful", result, Page: 1, PerPage: Int32.MaxValue, TotalElement: result.Count);
        }

        public ResponseHeader GetBookingByBookingId(int bookingId)
        {
            using var context = new SmartLockerContext();
            ResponseHeader response = new ResponseHeader();
            var result = context.Booking.Where(c => c.BookingId == bookingId).FirstOrDefault();
            if (result != null)
            {
                BookingDto content = mapper.Map<Booking, BookingDto>(result);
                var lkCode = context.LockerRooms.Where(c => c.LkRoomId == result.LkRoomId).FirstOrDefault();
                content.LkRoomCode = lkCode.LkRoomCode;
                response.Status = "S";
                response.Message = "Update booking to P sucessful";
                response.Content = content;
                response.Page = 1;
                response.PerPage = 1;
                response.TotalElement = 1;
            }
            else
            {
                response.Status = "F";
                response.Message = "BookingID invalid";
            }
            return response;
        }
        public ResponseHeader UpdateBooking(string status, int bookingId)
        {
            using var context = new SmartLockerContext();
            ResponseHeader response = new ResponseHeader();
            var result = context.Booking.Where(c => c.BookingId == bookingId).FirstOrDefault();
            if (result != null)
            {
                //update to P
                if (status == BookingStatus.Process)
                {
                    result.Status = BookingStatus.Process;
                    result.UpdateDate = DateTime.Now;
                    result.EndDate = null;
                    var updateResult = context.SaveChanges();
                    BookingDto content = mapper.Map<Booking, BookingDto>(result);
                    response.Status = "S";
                    response.Message = "Update booking to P sucessful";
                    response.Content = content;
                    response.Page = 1;
                    response.PerPage = 1;
                    response.TotalElement = 1;
                }
                //update to WF
                else if (status == BookingStatus.WaitFinish)
                {
                    if (result.Status != BookingStatus.Process)
                    {
                        throw new ConfilctDataException(@$"Cannot update WF Because BookingId {result.BookingId}'s status is not P");
                    }
                    else
                    {
                        result.Status = BookingStatus.WaitFinish;
                        result.UpdateDate = DateTime.Now;
                        result.EndDate = DateTime.Now.ToLocalTime();
                        result.PassCode = new Random().Next(100000, 999999).ToString();
                        var updateResult = context.SaveChanges();
                        if (updateResult == 0)
                        {
                            throw new Exception(@$"Cannot update booking status BookingId {bookingId}");
                        }
                        else
                        {
                            BookingDto content = mapper.Map<Booking, BookingDto>(result);
                            response.Status = "S";
                            response.Message = "Update booking to WF sucessful";
                            response.Content = content;
                            response.Page = 1;
                            response.PerPage = 1;
                            response.TotalElement = 1;
                        }

                    }
                }
                //update to R or NR
                else if (status == BookingStatus.Receiver || status == BookingStatus.NonReceiver)
                {
                    string statusResBooking = status == BookingStatus.Receiver ? BookingStatus.Process : BookingStatus.Delete;
                    string statusTransBooking = status == BookingStatus.Receiver ? BookingStatus.Tranfer : BookingStatus.Process;
                    var resBooking = context.Booking.Where(b => b.BookingId == bookingId).FirstOrDefault();
                    if (resBooking == null)
                    {
                        throw new ConfilctDataException("resbookingId Invalid");
                    }
                    resBooking.Status = statusResBooking;
                    resBooking.UpdateDate = DateTime.Now;

                    var transBooking = context.Transfers.Where(tr => tr.ResBookingId == resBooking.BookingId).FirstOrDefault();
                    transBooking.Status = status == BookingStatus.Receiver ? TransferStatus.TransferSuccess : TransferStatus.TransferReject;
                    transBooking.UpdateDate = DateTime.Now;

                    var transBookingResult = context.Booking.Where(b => b.BookingId == transBooking.TransBookingId).FirstOrDefault();
                    transBookingResult.Status = statusTransBooking;
                    transBookingResult.UpdateDate = DateTime.Now;

                    var updateBooking = context.SaveChanges();

                    response.Status = "S";
                    response.Message = "Update booking Transfer sucessful";
                    response.Content = resBooking;
                }
                //update to C
                else if (status == BookingStatus.Cancel)
                {

                    result.Status = BookingStatus.Cancel;
                    result.UpdateDate = DateTime.Now;
                    var lockerRoomUpdate = context.LockerRooms.Where(c => c.LkRoomId == result.LkRoomId).FirstOrDefault();

                    lockerRoomUpdate.Status = "A";
                    context.SaveChanges();

                    response.Status = "S";
                    response.Message = "cancel booking sucessful";
                }

                else
                {
                    return new ResponseHeader("F", "Status invalid");
                }
                return response;
            }
            else
            {
                return new ResponseHeader("F", "BookingID invalid");
            }

        }
        public ResponseHeader BookingRequestTransfer(int bookingId, int accountReceiveId)
        {
            using var context = new SmartLockerContext();
            var bookingTranfer = context.Booking.Where(b => b.BookingId == bookingId).FirstOrDefault();
            if (bookingTranfer == null)
            {
                return new ResponseHeader("F", "Booking not found");
            }
            bookingTranfer.Status = BookingStatus.WaitTranfer;
            bookingTranfer.UpdateDate = DateTime.Now;
            var update = context.SaveChanges();
            if (update == 0)
            {
                return new ResponseHeader("F", "Cannot Transfer");
            }
            Booking bookingReceiver = new Booking
            {
                AccountId = accountReceiveId,
                CreateDate = DateTime.Now,
                LkRoomId = bookingTranfer.LkRoomId,
                OverTime = bookingTranfer.OverTime,
                PassCode = "000000",
                RateTypeId = bookingTranfer.RateTypeId,
                StartDate = bookingTranfer.StartDate,
                Status = BookingStatus.WaitReceiver
            };
            var bookingReceiverAdd = context.Booking.Add(bookingReceiver).Entity;
            int addBooking = context.SaveChanges();
            if (addBooking == 0)
            {
                return new ResponseHeader("F", "Cannot Add to Receiver");
            }
            Transfers transfers = new Transfers
            {
                ResBookingId = bookingReceiverAdd.BookingId,
                TransBookingId = bookingTranfer.BookingId,
                CreateDate = DateTime.Now,
                Status = TransferStatus.TransferProcess
            };
            context.Transfers.Add(transfers);
            int addTransfer = context.SaveChanges();
            if (addTransfer == 0)
            {
                return new ResponseHeader("F", "Cannot Add Transfer");
            }
            return new ResponseHeader("S", "Transfer prepare Sucessful", bookingTranfer, Page: 1, PerPage: 1, TotalElement: 1);

        }
        public ResponseHeader GetAccountByResBookingId(int transBookingId)
        {
            using var context = new SmartLockerContext();
            var resBookingIdResult = context.Transfers.Where(tr => tr.TransBookingId == transBookingId).FirstOrDefault();
            if (resBookingIdResult == null)
            {
                throw new ConfilctDataException("transbookingId Invalid");
            }
            int resBookingId = resBookingIdResult.ResBookingId;
            var resAccountIdResult = context.Booking.Where(b => b.BookingId == resBookingId).FirstOrDefault().AccountId;
            var resAccountResult = context.Accounts.Where(c => c.AccountId == resAccountIdResult && c.Status == "A").FirstOrDefault();

            AccountDto result = mapper.Map<Accounts, AccountDto>(resAccountResult);
            return new ResponseHeader("S", "Get Account By BookingId Sucessful", result, Page: 1, PerPage: 1, TotalElement: 1);
        }
        public ResponseHeader GetAccountByTransBookingId(int resBookingId)
        {
            using var context = new SmartLockerContext();
            var transBookingIdResult = context.Transfers.Where(tr => tr.ResBookingId == resBookingId && tr.Status != "D").FirstOrDefault();
            if (transBookingIdResult == null)
            {
                throw new ConfilctDataException("resbookingId Invalid");
            }
            int transBookingId = transBookingIdResult.TransBookingId;
            var transAccountIdResult = context.Booking.Where(b => b.BookingId == transBookingId).FirstOrDefault().AccountId;
            var transAccountResult = context.Accounts.Where(c => c.AccountId == transAccountIdResult && c.Status == "A").FirstOrDefault();
            if (transAccountResult == null)
            {
                throw new ConfilctDataException("transAccountResult Invalid");
            }
            AccountDto result = mapper.Map<Accounts, AccountDto>(transAccountResult);
            return new ResponseHeader("S", "Get Account By BookingId Sucessful", result, Page: 1, PerPage: 1, TotalElement: 1);
        }
        public ResponseHeader CancelTransfer(int transBookingId)
        {
            using var context = new SmartLockerContext();

            var transferBooking = context.Booking.Where(tr => tr.BookingId == transBookingId).FirstOrDefault();
            transferBooking.Status = BookingStatus.Process;
            transferBooking.UpdateDate = DateTime.Now;

            var bookingTranfer = context.Transfers.Where(tr => tr.TransBookingId == transBookingId).FirstOrDefault();
            if (bookingTranfer == null)
            {
                throw new ConfilctDataException("transbookingId Invalid");
            }
            bookingTranfer.Status = "D";
            bookingTranfer.UpdateDate = DateTime.Now;

            int resBookingId = context.Transfers.Where(tr => tr.TransBookingId == transBookingId).FirstOrDefault().ResBookingId;
            var resbookingResult = context.Booking.Where(c => c.BookingId == resBookingId).FirstOrDefault();
            resbookingResult.Status = "D";
            resbookingResult.UpdateDate = DateTime.Now;

            var update = context.SaveChanges();
            if (update == 0)
            {
                throw new ConfilctDataException("Cannot update cancel transfer");
            }
            return new ResponseHeader("S", "Cancel transfer sucessful", transferBooking, Page: 1, PerPage: 1, TotalElement: 1);
        }
        public ResponseHeader UpdateResBooking(int resBookingId, string bookingStatus)
        {
            using var context = new SmartLockerContext();
            var resBooking = context.Booking.Where(b => b.BookingId == resBookingId).FirstOrDefault();
            if (resBooking == null)
            {
                throw new ConfilctDataException("resbookingId Invalid");
            }
            resBooking.Status = bookingStatus;
            var updateBooking = context.SaveChanges();
            if (updateBooking == 0)
            {
                throw new Exception("cannot update transfer booking");
            }
            return new ResponseHeader("S", "Update Receives transfer Booking sucessful");
        }


    }
}
