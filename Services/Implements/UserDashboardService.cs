using SmartLocker.Software.Backend.Models.Output.Dashboard;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class UserDashboardService : IUserDashboardService
    {
        public readonly IUserDashboardDA userDashboardDA;

        public UserDashboardService(IUserDashboardDA userDashboardDA)
        {
            this.userDashboardDA = userDashboardDA;
        }
        public UserDashboard GetUserDashboard(int accountId, string rangeType)
        {
            UserDashboard userDashboard = new UserDashboard();
            userDashboard.FinishUsing = userDashboardDA.CountFinishUseLocker(rangeType,accountId);

            UserDashboard transferUser = userDashboardDA.CountTranfer(rangeType, accountId);
            if(transferUser == null)
            {
                userDashboard.TotalTransfer = 0;
                userDashboard.SuccessTransfer = 0;
                userDashboard.RejectTransfer = 0;
            }
            else
            {
                userDashboard.TotalTransfer = transferUser.TotalTransfer;
                userDashboard.SuccessTransfer = transferUser.SuccessTransfer;
                userDashboard.RejectTransfer = transferUser.RejectTransfer;
            }
            

            return userDashboard;

        }
    }
}
