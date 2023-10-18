using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output.Dashboard;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class DashboardDA : IDashboardDA
    {
        private readonly IBaseRepository baseRepository;
        private readonly IBaseService baseService;
        public DashboardDA(IBaseRepository baseRepository, IBaseService baseService)
        {
            this.baseRepository = baseRepository;
            this.baseService = baseService;
        }
        
        public int LockerAllCount(int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT COUNT(*)
                              FROM FORM_REQUEST_LOCKERS FRL
                              WHERE 1=1
                              AND FRL.Status = 'A' ");
            if(accountId != null)
            {
                sql.Append($@" AND AccountId = {accountId} ");
            }

            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }
        public int LockerNRCount(int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT COUNT(*)
                              FROM FORM_REQUEST_LOCKERS FRL
                              WHERE 1=1
                              AND (FRL.Status <> 'A' AND FRL.Status <> 'R' ) ");
            if (accountId != null)
            {
                sql.Append($@" AND AccountId = {accountId} ");
            }

            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }
        public int LockerRepairCount(int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT Count(*) AS LockerRepair
                          FROM [SmartLocker].[dbo].[LOCKERS]
                          INNER JOIN FORM_REQUEST_LOCKERS ON FORM_REQUEST_LOCKERS.LockerId = LOCKERS.LockerId 
                          WHERE (
	                        SELECT COUNT(*) 
	                        FROM LOCKER_ROOMS 
	                        WHERE LOCKER_ROOMS.LockerId = LOCKERS.LockerId 
	                        AND LOCKER_ROOMS.Status ='F'
	                        ) > 0 
	                        AND LOCKERS.Status = 'A' ");
            if (accountId != null)
            {
                sql.Append($@" AND FORM_REQUEST_LOCKERS.AccountId = {accountId} ");
            }

            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }
        public int LocationAllCount(int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT COUNT(*)
                          FROM [SmartLocker].[dbo].[LOCATIONS]
                          WHERE Status = 'A'");
            if (accountId != null)
            {
                sql.Append($@" AND AccountId = {accountId} ");
            }

            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }
        public int LocationNRCount(int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT COUNT(*)
                          FROM [SmartLocker].[dbo].[LOCATIONS]
                          WHERE Status <> 'A' AND Status <> 'R'");
            if (accountId != null)
            {
                sql.Append($@" AND AccountId = {accountId} ");
            }

            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }
        
        public Decimal[] IncomeData(string rangeGraphType, int rateTypeId, int? accountId = null)
        {
            Decimal[] incomeResult = new Decimal[7];
            for (int i = 6; i >= 0; i--)
            {
                StringBuilder sql = new StringBuilder();
                int substractYear = 0;
                if (DateTime.Now.Year != DateTime.Now.AddMonths(0 - i).Year)
                {
                    substractYear = 1;
                }
                sql.Append($@"SELECT ISNULL(SUM(T.Amont),0)
                                 FROM TRANSACTIONS T
	                            INNER JOIN LOCKER_ROOMS LKR ON T.LkRoomId  = LKR.LkRoomId
                                INNER JOIN LOCKERS LK ON LKR.LockerId = LK.LockerId
                                INNER JOIN FORM_REQUEST_LOCKERS FRL ON FRL.LockerId = LK.LockerId
                                WHERE 1=1 ");
                switch (rangeGraphType)
                {
                    case RangeGraph.Week: sql.Append($@" AND CONVERT(VARCHAR(10), T.CreateDate, 103) = CONVERT(VARCHAR(10), (GETDATE()-{i}), 103) AND T.RateTypeId = {rateTypeId}  "); break;
                    case RangeGraph.Month: 
                        sql.Append($@" AND MONTH(T.CreateDate) = MONTH(DATEADD(month, {0 - i}, getdate()))
                                       AND YEAR(T.CreateDate) = YEAR(DATEADD(month, {0 - substractYear}, getdate())) AND T.RateTypeId = {rateTypeId} "); break;
                    case RangeGraph.Year: sql.Append($@" AND  YEAR(T.CreateDate) = YEAR(DATEADD(YEAR, {0 - i}, getdate())) AND T.RateTypeId = {rateTypeId} "); break;
                    default: throw new ArgumentException("Range graph is invalid");
                }
                if (accountId != null)
                {
                    sql.Append($@" AND FRL.AccountId = {accountId} ");
                }
                Decimal result = baseRepository.QueryString<Decimal>(sql.ToString()).FirstOrDefault();
                incomeResult[6 - i] = result;
            }
            return incomeResult;
            //return rangeGraphType switch
            //{
            //    RangeGraph.Week => WeekIncome(rateTypeId , accountId),
            //    RangeGraph.Month => MonthIncome(rateTypeId, accountId),
            //    RangeGraph.Year => YearIncome(rateTypeId, accountId),
            //    _ => throw new ArgumentException("rangeGraphType is invalid"),
            //};
        }

        public List<BookingCountChart> GetBookingLockerCountByAccountId(string rangeGraphType,int? monthRange , int? accountId = null )
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT TOP 5 L.LockerId,
		                        L.LockerCode,
		                        LO.LocateName,
		                        COUNT(B.BookingId) AS BookingCount
                          FROM LOCKERS L
                          INNER JOIN LOCKER_ROOMS LR ON LR.LockerId = L.LockerId
                          LEFT JOIN BOOKING B ON B.LkRoomId = LR.LkRoomId   
                           ");
            switch (rangeGraphType)
            {
                case RangeGraph.Day: sql.Append(" AND CONVERT(date, B.CreateDate)  = CONVERT(date, GETDATE()) ");break;
                case RangeGraph.Week: sql.Append(" AND CONVERT(date, B.CreateDate)  BETWEEN CONVERT(date,  GETDATE() - 7) AND CONVERT(date,  GETDATE())  ");break;
                case RangeGraph.Month: sql.Append($@" AND CONVERT(date, B.CreateDate)  BETWEEN CONVERT(date,  DATEADD(month, {0 - (monthRange ?? throw new ArgumentException("Month's range must not null."))} , getdate())) AND CONVERT(date,  GETDATE()) ");break;
                case RangeGraph.Year: sql.Append(" AND  CONVERT(date, B.CreateDate) BETWEEN CONVERT(date,  DATEADD(year, -1 , getdate())) AND CONVERT(date,  GETDATE()) ");break;
                default:throw new ArgumentException("Range graph is invalid");
            }
            sql.Append($@" INNER JOIN LOCATIONS LO ON LO.LocateId = L.LocateId
                          INNER JOIN FORM_REQUEST_LOCKERS FRL ON FRL.LockerId = L.LockerId
                          WHERE L.Status = 'A' ");
            if (accountId != null)
            {
                sql.Append($@"   AND FRL.AccountId = {accountId} ");
            }
            sql.Append($@"   GROUP BY L.LockerId ,L.LockerCode,
		                                LO.LocateName
                           ORDER BY BookingCount DESC ");
            return baseRepository.QueryString<BookingCountChart>(sql.ToString()).ToList();
        }
        
        public List<BookingCountChart> GetBookingLocationCountByAccountId(string rangeGraphType, int? monthRange, int? accountId = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT TOP 5 L.LocateName
                               ,COUNT(B.BookingId) AS BOOKINGCOUNT
                          FROM [SmartLocker].[dbo].[LOCATIONS] L
                          LEFT JOIN LOCKERS  LK ON L.LocateId = LK.LocateId 
                          LEFT JOIN LOCKER_ROOMS LR ON LK.LockerId = LR.LockerId
                          LEFT JOIN BOOKING B ON B.LkRoomId = LR.LkRoomId
                           ");
            switch (rangeGraphType)
            {
                case RangeGraph.Day: sql.Append(" AND CONVERT(date, B.CreateDate)  = CONVERT(date, GETDATE()) "); break;
                case RangeGraph.Week: sql.Append(" AND CONVERT(date, B.CreateDate)  BETWEEN CONVERT(date,  GETDATE() - 7) AND CONVERT(date,  GETDATE())  "); break;
                case RangeGraph.Month: sql.Append($@" AND CONVERT(date, B.CreateDate)  BETWEEN CONVERT(date,  DATEADD(month, {0 - (monthRange ?? throw new ArgumentException("Month's range must not null."))} , getdate())) AND CONVERT(date,  GETDATE()) "); break;
                case RangeGraph.Year: sql.Append(" AND  CONVERT(date, B.CreateDate) BETWEEN CONVERT(date,  DATEADD(year, -1 , getdate())) AND CONVERT(date,  GETDATE()) "); break;
                default: throw new ArgumentException("Range graph is invalid");
            }
            sql.Append($@"  WHERE L.Status = 'A' ");
            if (accountId != null)
            {
                sql.Append($@"  AND L.AccountId = {accountId} ");
            }
            sql.Append($@" GROUP BY L.LocateName
                           ORDER BY BOOKINGCOUNT DESC ");
            return baseRepository.QueryString<BookingCountChart>(sql.ToString()).ToList();
        }


    }
}
