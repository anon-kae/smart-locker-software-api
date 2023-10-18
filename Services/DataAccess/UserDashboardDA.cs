using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Models.Output.Dashboard;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class UserDashboardDA : IUserDashboardDA
    {
        private readonly IBaseRepository baseRepository;
        public UserDashboardDA(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public int CountFinishUseLocker(string rangeGraphType , int accountId)
        {
            var range = "";
            range = rangeGraphType switch
            {
                RangeGraph.Week => "DAY",
                RangeGraph.Month => "MONTH",
                RangeGraph.Year => "YEAR",
                _ => throw new ArgumentException("Range graph is invalid"),
            };
            string sql = $@"SELECT COUNT(*)
                              FROM [SmartLocker].[dbo].[BOOKING]
                              where
                                AccountId = '{accountId}'
                              and
                              Status = 'F' and
                              [BOOKING].CreateDate BETWEEN DATEADD({range} ,-7,GETDATE())  AND  DATEADD({range} ,0,GETDATE())";

            int result = baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
            return result;
        }

        public UserDashboard CountTranfer(string rangeGraphType, int accountId)
        {
            var range = "";
            UserDashboard user = new UserDashboard();

            range = rangeGraphType switch
            {
                RangeGraph.Week => "DAY",
                RangeGraph.Month => "MONTH",
                RangeGraph.Year => "YEAR",
                _ => throw new ArgumentException("Range graph is invalid"),
            };
            //string sql = $@"SELECT [TransBookingId] As AccountId,
            //                      count(*) totalTransfer,
            //                      sum(case when Status = 'S' then 1 else 0 end) as successTransfer,
            //                      sum(case when Status = 'R' then 1 else 0 end) as rejectTransfer
            //                      FROM [SmartLocker].[dbo].[TRANSFERS]
            //                      where 
            //                        [TransBookingId] = '{accountId}'
            //                     and
            //                      CreateDate BETWEEN DATEADD({range} ,-7,GETDATE())  AND  DATEADD({range} ,0,GETDATE())
            //                      GROUP BY [TransBookingId]";
            string sql = $@"SELECT count(*)	
                                  FROM [SmartLocker].[dbo].[TRANSFERS]
                                    INNER JOIN BOOKING ON BOOKING.BookingId = [TRANSFERS].TransBookingId
                                  where 
                                    BOOKING.AccountId = '{accountId}'
                                 and
                                  [TRANSFERS].CreateDate BETWEEN DATEADD({range} ,-7,GETDATE())  AND  DATEADD({range} ,0,GETDATE())";

            int result = baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
            user.TotalTransfer = result;
            sql = $@"SELECT count(*)	
                                  FROM [SmartLocker].[dbo].[TRANSFERS]
                                    INNER JOIN BOOKING ON BOOKING.BookingId = [TRANSFERS].TransBookingId
                                  where 
                                    BOOKING.AccountId = '{accountId}'
                                    AND [TRANSFERS].Status = 'R'
                                 and
                                  [TRANSFERS].CreateDate BETWEEN DATEADD({range} ,-7,GETDATE())  AND  DATEADD({range} ,0,GETDATE())";
            result = baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
            user.RejectTransfer = result;
            sql = $@"SELECT count(*)	
                                  FROM [SmartLocker].[dbo].[TRANSFERS]
                                    INNER JOIN BOOKING ON BOOKING.BookingId = [TRANSFERS].TransBookingId
                                  where 
                                    BOOKING.AccountId = '{accountId}'
                                    AND BOOKING.Status  = 'T'
                                 and
                                  [TRANSFERS].CreateDate BETWEEN DATEADD({range} ,-7,GETDATE())  AND  DATEADD({range} ,0,GETDATE())";
            result = baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
            user.SuccessTransfer = result;
            return user;
        }

        
    }
}
