using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IDashboardDA
    {
        int LockerAllCount(int? accountId = null);
        int LockerNRCount(int? accountId = null);
        int LockerRepairCount(int? accountId = null);
        int LocationAllCount(int? accountId = null);
        int LocationNRCount(int? accountId = null);
        Decimal[] IncomeData(string rangeGraphType,int rateTypeId, int? accountId = null);
        List<BookingCountChart> GetBookingLockerCountByAccountId(string rangeGraphType, int? monthRange, int? accountId = null);
        List<BookingCountChart> GetBookingLocationCountByAccountId(string rangeGraphType, int? monthRange, int? accountId = null);

    }
}
