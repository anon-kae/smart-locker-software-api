using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
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
    public class RepairFormDA : IRepairFormDA
    {
        private readonly IBaseRepository baseRepository;
        private readonly IBaseService baseService;
        public RepairFormDA(IBaseRepository baseRepository, IBaseService baseService)
        {
            this.baseRepository = baseRepository;
            this.baseService = baseService;
        }

        public PageModel<RepairFormDto> GetRepairForm(int page, int perPage, RepairFormDto repairFormDto)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT DISTINCT RF.[RepairFormId]
                                          ,RF.[Status]
                                          ,RF.[CreateDate]
                                          ,RF.[UpdateDate]
                                          ,RF.[Remark]
                                          ,RF.[AccountId]
	                                      ,ACC.FirstName
	                                      ,ACC.LastName
	                                      ,L.LockerCode
                                      FROM [SmartLocker].[dbo].[REPAIR_FORM] RF
                                      INNER JOIN ACCOUNTS ACC ON ACC.AccountId = RF.AccountId AND ACC.Status = 'A'
                                      INNER JOIN REPAIR_LOCKER_ROOM RLR ON RLR.RepairFormId = RF.RepairFormId
                                      INNER JOIN LOCKER_ROOMS LR ON LR.LkRoomId = RLR.LkRoomId
                                      INNER JOIN LOCKERS L ON L.LockerId = LR.LockerId
                                      WHERE ( 
                                      L.LockerCode LIKE '%{repairFormDto.LockerCode}%' 
                                      AND ACC.FirstName LIKE '%{repairFormDto.FirstName}%'
                                      AND ACC.LastName LIKE '%{repairFormDto.LastName}%'");
            if (!String.IsNullOrEmpty(repairFormDto.Status))
            {
                sql.Append($@" AND RF.Status = '{repairFormDto.Status}' ");
            }
            if (repairFormDto.AccountId > 0)
            {
                sql.Append($@" AND RF.[AccountId] = '{repairFormDto.AccountId}' ");
            }
            sql.Append(" ) ORDER BY RF.CreateDate DESC ");

            PageModel<RepairFormDto> pageModel = baseService.Pagination<RepairFormDto>(page, perPage, sql.ToString());
            return pageModel;
        }

        public List<int> GetSummaryRepair(RepairFormDto repairFormDto)
        {
            List<int> res = new List<int>();
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            StringBuilder sql3 = new StringBuilder();
            StringBuilder sql4 = new StringBuilder();
            sql1.Append($@"SELECT COUNT(RF.[Status])
                        FROM [SmartLocker].[dbo].[REPAIR_FORM] RF
                        INNER JOIN ACCOUNTS ACC ON ACC.AccountId = RF.AccountId AND ACC.Status = 'A'
                        INNER JOIN REPAIR_LOCKER_ROOM RLR ON RLR.RepairFormId = RF.RepairFormId
                        WHERE RF.[Status] = 'R'");
            sql2.Append($@"SELECT COUNT(RF.[Status])
                        FROM [SmartLocker].[dbo].[REPAIR_FORM] RF
                        INNER JOIN ACCOUNTS ACC ON ACC.AccountId = RF.AccountId AND ACC.Status = 'A'
                        INNER JOIN REPAIR_LOCKER_ROOM RLR ON RLR.RepairFormId = RF.RepairFormId
                        WHERE RF.[Status] = 'P'");
            sql3.Append($@"SELECT COUNT(RF.[Status])
                        FROM [SmartLocker].[dbo].[REPAIR_FORM] RF
                        INNER JOIN ACCOUNTS ACC ON ACC.AccountId = RF.AccountId AND ACC.Status = 'A'
                        INNER JOIN REPAIR_LOCKER_ROOM RLR ON RLR.RepairFormId = RF.RepairFormId
                        WHERE RF.[Status] = 'A'");
            sql4.Append($@"SELECT COUNT(RF.[Status])
                        FROM [SmartLocker].[dbo].[REPAIR_FORM] RF
                        INNER JOIN ACCOUNTS ACC ON ACC.AccountId = RF.AccountId AND ACC.Status = 'A'
                        INNER JOIN REPAIR_LOCKER_ROOM RLR ON RLR.RepairFormId = RF.RepairFormId
                        WHERE RF.[Status] = 'W' or RF.[Status] = 'VF'");
            if (repairFormDto.AccountId > 0)
            {
                sql1.Append($@" AND RF.[AccountId] = '{repairFormDto.AccountId}' ");
                sql2.Append($@" AND RF.[AccountId] = '{repairFormDto.AccountId}' ");
                sql3.Append($@" AND RF.[AccountId] = '{repairFormDto.AccountId}' ");
                sql4.Append($@" AND RF.[AccountId] = '{repairFormDto.AccountId}' ");
            }

            int statusR = baseRepository.QueryString<int>(sql1.ToString()).FirstOrDefault();
            int statusA = baseRepository.QueryString<int>(sql3.ToString()).FirstOrDefault();
            int statusP = baseRepository.QueryString<int>(sql2.ToString()).FirstOrDefault();
            int statusVFORW = baseRepository.QueryString<int>(sql4.ToString()).FirstOrDefault();
            res.Add(statusA);
            res.Add(statusP);
            res.Add(statusVFORW);
            res.Add(statusR);
            
            return res;
        }

        public List<RepairLkRoomDto> GetRepairLkRoomList(int repairFormId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@"SELECT RLR.[RepairLkRoomId]
                              ,RLR.[LkRoomId]
                              ,RLR.[Status]
                              ,RLR.[CreateDate]
                              ,RLR.[UpdateDate]
                              ,RLR.[RepairFormId]
                              ,Remark
	                          ,LR.LkRoomCode
                          FROM [SmartLocker].[dbo].[REPAIR_LOCKER_ROOM] RLR
                          INNER JOIN LOCKER_ROOMS LR ON LR.LkRoomId = RLR.LkRoomId
                          WHERE RepairFormId = {repairFormId}  ORDER BY RLR.RepairLkRoomId ");
            List<RepairLkRoomDto> pageModel = baseRepository.QueryString<RepairLkRoomDto>(sql.ToString()).ToList();
            return pageModel;
        }
    }
}
