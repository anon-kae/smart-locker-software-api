using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IDashBoardService
    {
        ResponseHeader GetLocateAndLockerResultByAccountId(int AccountId);
        ResponseHeader IncomeResult(DashboardRequireModel dashboardRequireModel);
        ResponseHeader GetBookingLockerCountByAccountId(DashboardRequireModel dashboardRequireModel);
        ResponseHeader GetBookingLocationCountByAccountId(DashboardRequireModel dashboardRequireModel);
    }
}
