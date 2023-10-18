using SmartLocker.Software.Backend.Models.Output.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IUserDashboardDA
    {
        int CountFinishUseLocker(string rangeGraphType, int accountId);
        UserDashboard CountTranfer(string rangeGraphType, int accountId);
    }
}
