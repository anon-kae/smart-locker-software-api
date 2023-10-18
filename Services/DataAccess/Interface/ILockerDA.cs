using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface ILockerDA
    {
        List<ServiceChargeDto> GetAllServiceCharge();

        List<LockerRoomDto> GetAllLockerRoom(LockerRoomDto lockerRoomDto);

        LockerRooms GetLockerRoomByLkRoomId(int lkRoomId);
        List<LockerSizes> GetLockerSize();
        PageModel<LockerDto> GetLockerByFilter(int page, int perPage, LockerDto lockerDto);
    }
}
