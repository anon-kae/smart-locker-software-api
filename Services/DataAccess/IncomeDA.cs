using SmartLocker.Software.Backend.Constants;
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
    public class IncomeDA : IIncomeDA
    {
        private readonly IBaseService baseService;
        private readonly IBaseRepository baseRepository;

        public IncomeDA(IBaseService baseService, IBaseRepository baseRepository)
        {
            this.baseService = baseService;
            this.baseRepository = baseRepository;
        }

        public int GetIncome(DateTime date, string dateLength, int? locateId = null, int? lockerId = null, int? lkRoomId = null)
        {
            StringBuilder sql = new StringBuilder();
            string dateFormat = date.ToString("dd-MM-yyyy");
            sql.Append($@" SELECT ISNULL(SUM(T.Amont),0) 
                            FROM TRANSACTIONS T 
                            INNER JOIN LOCKER_ROOMS LR ON LR.LkRoomId = T.LkRoomId 
                            INNER JOIN LOCKERS LK ON LK.LockerId = LR.LockerId And LK.Status = 'A' 
                            INNER JOIN LOCATIONS LO ON LO.LocateId = LK.LocateId  AND LO.Status = 'A' 
                            INNER JOIN ADDRESS AD ON AD.AddressId = LO.AddressId 
                            INNER JOIN FORM_REQUEST_LOCKERS FRL ON FRL.LockerId = LK.LockerId 
                            INNER JOIN ACCOUNTS ACC ON ACC.AccountId  = FRL.AccountId 
                            WHERE 1=1 
                             ") ;
            if (locateId.HasValue)
            {
                sql.Append($@" AND LO.LocateId = {locateId.Value} ");
            }else if (lockerId.HasValue)
            {
                sql.Append($@" AND LK.LockerId = {lockerId.Value} ");
            }
            else if (lkRoomId.HasValue)
            {
                sql.Append($@" AND LR.LkRoomId = {lkRoomId.Value} ");
            }else
            {
                throw new ArgumentException("this function must have send at least one parameter (locateId , lockerId , lkRoomId)");
            }
            switch (dateLength)
            {
                case RangeGraph.Week: sql.Append($@" AND CONVERT(datetime, T.CreateDate , 103) = CONVERT(datetime, '{dateFormat.Trim()}' , 103) ");break;
                case RangeGraph.Month: sql.Append($@" AND MONTH(T.CreateDate) = MONTH(CONVERT(datetime, '{dateFormat.Trim()}' , 103))                                      
							                            AND YEAR(T.CreateDate) = YEAR(CONVERT(datetime, '{dateFormat.Trim()}' , 103))");break;
                case RangeGraph.Year: sql.Append($@" AND YEAR(T.CreateDate) = YEAR(CONVERT(datetime, '{dateFormat.Trim()}' , 103)) ");break;
                default:throw new ArgumentException("DateLength in DA is invalid");
            }
            return baseRepository.QueryString<int>(sql.ToString()).FirstOrDefault();
        }


        

       
    }
}
