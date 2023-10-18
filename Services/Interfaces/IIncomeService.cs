

using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IIncomeService
    {
        ResponseHeader GetIncomeLocation(int page, int perPage, IncomeDto incomeDto);
        ResponseHeader GetIncomeLocker(int page, int perPage, IncomeLockerDto incomeDto);
        ResponseHeader GetIncomeLockerRoom(IncomeLockerRoomDto incomeLockerRoomDto);
        string GetIncomeLocationExcel(int page, int perPage, IncomeDto income);
        string GetIncomeLockerExcel(int page, int perPage, IncomeLockerDto incomeLockerDto);
        string GetIncomeLockerRoomExcel(IncomeLockerRoomDto incomeLockerRoomDto);
    }
}
