using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SmartLocker.Software.Backend.Entities;
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
    public class LockerDA : ILockerDA
    {
        private readonly IBaseRepository baseRepository;
        private readonly IBaseService baseService;

        public LockerDA(IBaseRepository baseRepository , IBaseService baseService)
        {
            this.baseRepository = baseRepository;
            this.baseService = baseService;
        }

        public List<LockerRoomDto> GetAllLockerRoom(LockerRoomDto lockerRoomDto)
        {
            StringBuilder sql = new StringBuilder();
             sql.Append($@"SELECT LR.[LkRoomId]
                                  ,[LkRoomCode]
                                  ,[LockerId]
                                  ,LR.[LkSizeId]
                                  ,LR.[Status]
                                  ,[LkSizeName]        
                                    ,[ColumnPosition]
                                  ,[RowPosition] FROM LOCKER_ROOMS  LR
                                        INNER JOIN LOCKER_SIZES LS ON LR.LkSizeId = LS.LkSizeId AND LS.Status = 'A' 
                                        INNER JOIN LOCKER_DIAGRAMS LD ON LR.LkRoomId = LD.LkRoomId AND LD.Status = 'A' 
                            WHERE LR.Status <> 'I' ");
            if(lockerRoomDto.LockerId > 0)
            {
                sql.Append($@" AND LR.LockerId = {lockerRoomDto.LockerId} ");
            }
            if (!String.IsNullOrEmpty(lockerRoomDto.Status))
            {
                sql.Append($@" AND LR.Status = '{lockerRoomDto.Status}' ");
            }
            return baseRepository.QueryString<LockerRoomDto>(sql.ToString()).ToList();
        }

        public List<ServiceChargeDto> GetAllServiceCharge()
        {
            string sql = $@"SELECT * FROM SERVICE_RATE  SR
                                        INNER JOIN RATE_TYPES RT ON SR.TypeId = RT.TypeId AND RT.Status = 'A' 
                                        INNER JOIN LOCKER_SIZES LS ON SR.LkSizeId = LS.LkSizeId AND LS.Status = 'A' 
                                        WHERE SR.Status = 'A'";
            return baseRepository.QueryString<ServiceChargeDto>(sql).ToList();
        }

        public LockerRooms GetLockerRoomByLkRoomId(int lkRoomId)
        {
            using var context = new SmartLockerContext();
            return context.LockerRooms.Where(c => c.LkRoomId == lkRoomId).FirstOrDefault();
        }

        public List<LockerSizes> GetLockerSize()
        {
            string sql = $@"SELECT * FROM [LOCKER_SIZES]";
            return baseRepository.QueryString<LockerSizes>(sql).ToList();
        }

        public PageModel<LockerDto> GetLockerByFilter(int page, int perPage , LockerDto lockerDto)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append($@"SELECT LK.LockerId
                                  ,LK.LockerCode
                              FROM [SmartLocker].[dbo].[LOCKERS] LK
                              INNER JOIN LOCATIONS LO ON LO.LocateId = LK.LocateId
                              WHERE LK.Status = 'A' AND LockerCode LIKE '%{lockerDto.LockerCode}%' AND LO.LocateId = {(lockerDto.LocateId > 0 ? lockerDto.LocateId : throw new ArgumentException("locate id is invalid"))} 
                              ORDER BY LK.UpdateDate DESC, LK.CreateDate DESC");

            PageModel<LockerDto> pageModel = baseService.Pagination<LockerDto>(page, perPage, sql.ToString());
            return pageModel;
        }



    }
}
