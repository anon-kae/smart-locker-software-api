using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface ILockerService
    {
        ResponseHeader GetAllServiceCharge();
        ResponseHeader GetServiceChargeByLockerRoomId(int lockerRoomId, int typeId);
        ResponseHeader GetAllLockerRoomByLockerId(int id);
        ResponseHeader GetAllType();
        ResponseHeader GetAllSize();
        ResponseHeader GetTypeByTypeId(int typeId);
        ResponseHeader GetLockerByLocateId(int LocateId);

        ResponseHeader GetLockerRoomByLkRoomId(int lkRoomId);

    }
}
