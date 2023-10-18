using SmartLocker.Software.Backend.Models.Output.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IUserDashboardService
    {
        UserDashboard GetUserDashboard(int accountId, string rangeType);
    }
}
