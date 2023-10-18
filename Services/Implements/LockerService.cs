using AutoMapper;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class LockerService : ILockerService
    {
        private readonly ILockerDA lockerDA;
        private readonly IMapper mapper;

        public LockerService(ILockerDA lockerDA, IMapper mapper)
        {
            this.lockerDA = lockerDA;
            this.mapper = mapper;
        }

        public ResponseHeader GetAllLockerRoomByLockerId(int id)
        {
            var LockerRoomResult = lockerDA.GetAllLockerRoom(new LockerRoomDto() { LockerId = id });
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get all LockerRoom By LockerId sucessful",
                Content = LockerRoomResult,
                Page = 1,
                PerPage = int.MaxValue,
                TotalElement = LockerRoomResult.Count
            };
        }

        public ResponseHeader GetAllServiceCharge()
        {
            var ServiceRateResult = lockerDA.GetAllServiceCharge();
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get all service Charge sucessful",
                Content = ServiceRateResult,
                Page = 1,
                PerPage = int.MaxValue,
                TotalElement = ServiceRateResult.Count
            };
        }

        public ResponseHeader GetServiceChargeByLockerRoomId(int lockerRoomId, int typeId)
        {
            using var context = new SmartLockerContext();
            int lockerRoomResult = context.LockerRooms.Where(c => c.Status != "D" && c.LkRoomId == lockerRoomId).FirstOrDefault().LkSizeId;
            ServiceRate serviceRate = context.ServiceRate.Where(c => c.LkSizeId == lockerRoomResult && c.TypeId == typeId).FirstOrDefault();
            return new ResponseHeader()
            {
                Status = "S",
                Message = " Get ServiceCharge By LockerRoomId",
                Content = serviceRate,
                Page = 1,
                PerPage = 1,
                TotalElement = 1
            };
        }
        public ResponseHeader GetAllType()
        {
            using var context = new SmartLockerContext();
            var typeResult = context.RateTypes.Where(c => c.Status == "A").ToList();
            List<RateTypeDto> rateTypeDtos = mapper.Map<List<RateTypes>, List<RateTypeDto>>(typeResult);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get all service Charge sucessful",
                Content = typeResult,
                Page = 1,
                PerPage = int.MaxValue,
                TotalElement = typeResult.Count
            };
        }

        public ResponseHeader GetTypeByTypeId(int typeId)
        {
            using var context = new SmartLockerContext();
            var typeResult = context.RateTypes.Where(c => c.Status == "A" && c.TypeId == typeId).FirstOrDefault();
            RateTypeDto rateTypeDtos = mapper.Map<RateTypes, RateTypeDto>(typeResult);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Type By TypeId sucessful",
                Content = typeResult,
                Page = 1,
                PerPage = 1,
                TotalElement = 1
            };
        }

        public ResponseHeader GetLockerByLocateId(int locateId)
        {
            using var context = new SmartLockerContext();
            var typeResult = context.Lockers.Where(c => c.LocateId == locateId && c.Status == "A").ToList();
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Locker By LocateId sucessful",
                Content = typeResult,
                Page = 1,
                PerPage = 1,
                TotalElement = 1
            };
        }
        public ResponseHeader GetLockerRoomByLkRoomId(int lkRoomId)
        {
            LockerRooms lockerRoomsResult = lockerDA.GetLockerRoomByLkRoomId(lkRoomId);
            LockerRoomDto roomDto = mapper.Map<LockerRooms, LockerRoomDto>(lockerRoomsResult);
            return new ResponseHeader("S", "Get LockerRoom by lkRoomId successful", roomDto);
        }

        public ResponseHeader GetAllSize()
        {
            List<LockerSizes> lockerRoomsResult = lockerDA.GetLockerSize().ToList();
            return new ResponseHeader("S", "Get LockerSize successful", lockerRoomsResult);
        }
    }
}
