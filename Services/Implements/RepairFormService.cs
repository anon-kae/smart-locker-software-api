using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class RepairFormService : IRepairFormService
    {
        private readonly IRepairFormDA repairFormDA;
        private readonly IBaseService baseService;
        
        private readonly ILockerDA lockerDA;
        public RepairFormService(IRepairFormDA repairFormDA , ILockerDA lockerDA , IBaseService baseService)
        {
            this.repairFormDA = repairFormDA;
            this.lockerDA = lockerDA;
            this.baseService = baseService;
        }
        public ResponseHeader AddRepairForm(RepairFormDto repairFormDto)
        {
            using var context = new SmartLockerContext();
            //add repair form
            ResponseHeader responseHeader = new ResponseHeader();
            List<RepairLockerRoom> repairLockerRoomList = new List<RepairLockerRoom>();
            RepairForm newRepairForm = new RepairForm
            {
                AccountId = repairFormDto.AccountId,
                Status = "VF",
                CreateDate = DateTime.Now,
            };
            context.RepairForm.Add(newRepairForm);
            int addFormResult = context.SaveChanges();

            if (!(addFormResult > 0))
            {
                throw new Exception("Cannot insert right now.");
            }
            else
            {
                //add repair lkRoom
                foreach (int lkRoomId in repairFormDto.LkRoomIdList)
                {
                    RepairLockerRoom repairLockerRoom = new RepairLockerRoom()
                    {
                        LkRoomId = lkRoomId,
                        Status = "W",
                        CreateDate = DateTime.Now,
                        RepairFormId = newRepairForm.RepairFormId
                    };
                    repairLockerRoomList.Add(repairLockerRoom);
                }
                context.RepairLockerRoom.AddRange(repairLockerRoomList);
                context.SaveChanges();

                //CHANGE LKROOM STATUS 
                foreach (int lkRoomId in repairFormDto.LkRoomIdList)
                {
                    var LkRoomUpdate = context.LockerRooms
                        .Where(c => c.LkRoomId == lkRoomId)
                        .FirstOrDefault();
                    LkRoomUpdate.Status = "F";
                    LkRoomUpdate.UpdateDate = DateTime.Now;
                    context.SaveChanges();
                }
                responseHeader.Status = "S";
                responseHeader.Message = "Add report form successful";
            }


            return responseHeader;
        }
        public ResponseHeader GetRepairForm(int page, int perPage, RepairFormDto repairFormDto)
        {
            string roleResult = baseService.CheckAccountRoleByAccountId(repairFormDto.AccountId);

            switch (roleResult)
            {
                case "400": throw new ArgumentException("AccountId is invalid.");
                case "409": throw new ConfilctDataException("Role is conflict data.");
                case "User": throw new ArgumentException("User cannot use this function");
                case "Admin":
                    repairFormDto.AccountId = 0; break;
                case "Partner":
                     break;
                default: throw new Exception("Something when wrong");
            };
            var repairResult = repairFormDA.GetRepairForm(page, perPage, repairFormDto);
            List<RepairFormDto> content = repairResult.Content;
            foreach (RepairFormDto repairForm in content)
            {
                repairForm.RepairLkRoomList = repairFormDA.GetRepairLkRoomList(repairForm.RepairFormId);
            }
            ResponseHeader responseHeader = new ResponseHeader
            {
                Status = "S",
                Message = "get report form successful",
                Content = content,
                Page = page,
                PerPage = perPage,
                TotalElement = repairResult.TotalElement
            };
            return responseHeader;
        }
        public ResponseHeader ApproveRepairForm(RepairFormDto repairFormDto)
        {
            using var context = new SmartLockerContext();

            //validate exist data
            var RepairFormIsvalid = context.RepairForm.Where(c => c.RepairFormId == repairFormDto.RepairFormId && c.Status.Equals("VF")).Count();
            if (RepairFormIsvalid == 0)
            {
                throw new ArgumentException("RepairFormId is invalid or is not VF status ");
            }
            foreach (RepairLkRoomDto repairLkRoomDto in repairFormDto.RepairLkRoomList)
            {
                if(!(repairLkRoomDto.Status.Equals(RepairLkRoomStatus.Process) || repairLkRoomDto.Status.Equals(RepairLkRoomStatus.Reject)))
                {
                    throw new ArgumentException($@"status in {repairLkRoomDto.RepairFormId} is invalid. It's {repairLkRoomDto.Status}");
                }

                var RepairLkRoomisValid = context.RepairLockerRoom
                    .Where(c => c.RepairLkRoomId == repairLkRoomDto.RepairLkRoomId && c.RepairFormId == repairFormDto.RepairFormId )
                    .Count();

                if (RepairLkRoomisValid == 0 )
                {
                    {
                        throw new ArgumentException($@"LockerRoomId : {repairLkRoomDto.RepairFormId} in this repairFormId is invalid");
                    }
                }
            }

            //change repair form status 
            var RepairFormResult = context.RepairForm.Where(c => c.RepairFormId == repairFormDto.RepairFormId).FirstOrDefault();
            RepairFormResult.Status = repairFormDto.Status;
            RepairFormResult.Remark = repairFormDto.Remark;
            RepairFormResult.UpdateDate = DateTime.Now;
            context.SaveChanges();

            //change repair locker room status 
            foreach (RepairLkRoomDto repairLkRoomDto in repairFormDto.RepairLkRoomList)
            {
                var RepairLkRoomResult = context.RepairLockerRoom
                    .Where(c => c.RepairLkRoomId == repairLkRoomDto.RepairLkRoomId && c.RepairFormId == repairFormDto.RepairFormId)
                    .FirstOrDefault();
                RepairLkRoomResult.Status = repairLkRoomDto.Status;
                RepairLkRoomResult.Remark = repairLkRoomDto.Remark;
                RepairLkRoomResult.UpdateDate = DateTime.Now;
                context.SaveChanges();

                //if reject change lk room status to A
                if (RepairLkRoomResult.Status.Equals(RepairLkRoomStatus.Reject))
                {
                    var lkRoomBacktoReady = context.LockerRooms.Where(c => c.LkRoomId == RepairLkRoomResult.LkRoomId).FirstOrDefault();
                    lkRoomBacktoReady.Status = "A";
                    lkRoomBacktoReady.UpdateDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            ResponseHeader responseHeader = new ResponseHeader()
            {
                Status = "S",
                Message = "Approve Repair Form sucessful"
            };
            return responseHeader;
        }
        public ResponseHeader FinishRepairForm(RepairFormDto repairFormDto)
        {
            using var context = new SmartLockerContext();

            //validate exist data
            var RepairFormIsvalid = context.RepairForm.Where(c => c.RepairFormId == repairFormDto.RepairFormId && c.Status.Equals("P")).Count();
            if (RepairFormIsvalid == 0)
            {
                throw new ArgumentException("RepairFormId is invalid or is not P status ");
            }

            //change repair form status 
            var RepairFormResult = context.RepairForm.Where(c => c.RepairFormId == repairFormDto.RepairFormId).FirstOrDefault();
            RepairFormResult.Status = "A";
            RepairFormResult.UpdateDate = DateTime.Now;
            context.SaveChanges();

            //change repair locker room status 
            List<RepairLockerRoom> RepairLkRoomList = context.RepairLockerRoom
                .Where(c => c.RepairFormId == repairFormDto.RepairFormId && c.Status
                    .Equals(RepairLkRoomStatus.Process))
                .ToList();
            foreach (RepairLockerRoom repairLkRoom in RepairLkRoomList)
            {
                var RepairLkRoomResult = context.RepairLockerRoom
                    .Where(c => c.RepairLkRoomId == repairLkRoom.RepairLkRoomId)
                    .FirstOrDefault();
                RepairLkRoomResult.Status = RepairLkRoomStatus.Finish;
                RepairLkRoomResult.UpdateDate = DateTime.Now;
                context.SaveChanges();

                var lkRoomBacktoReady = context.LockerRooms.Where(c => c.LkRoomId == RepairLkRoomResult.LkRoomId).FirstOrDefault();
                    lkRoomBacktoReady.Status = "A";
                    lkRoomBacktoReady.UpdateDate = DateTime.Now;
                    context.SaveChanges();
                
            }
            ResponseHeader responseHeader = new ResponseHeader()
            {
                Status = "S",
                Message = "Finish Repair Form sucessful"
            };
            return responseHeader;
        }
        public ResponseHeader GetLockerRoomReadyToRepair(LockerRoomDto lockerRoomDto)
        {

            var lockerRoomResult = lockerDA.GetAllLockerRoom(lockerRoomDto);
            if(lockerRoomResult.Count == 0)
            {
                throw new ArgumentException("Locker Id is invalid.");
            }

            ResponseHeader responseHeader = new ResponseHeader
            {
                Status = "S",
                Message = "get LockerRoomReadyToRepair successful",
                Content = lockerRoomResult,
                Page = 1,
                PerPage = Int32.MaxValue,
                TotalElement = lockerRoomResult.Count
            };
            return responseHeader;
        }

        public ResponseHeader GetSummaryRepair(RepairFormDto repairFormDto)
        {
            string roleResult = baseService.CheckAccountRoleByAccountId(repairFormDto.AccountId);

            switch (roleResult)
            {
                case "400": throw new ArgumentException("AccountId is invalid.");
                case "409": throw new ConfilctDataException("Role is conflict data.");
                case "User": throw new ArgumentException("User cannot use this function");
                case "Admin":
                    repairFormDto.AccountId = 0; break;
                case "Partner":
                     break;
                default: throw new Exception("Something when wrong");
            };
            var repairResult = repairFormDA.GetSummaryRepair(repairFormDto);
            ResponseHeader responseHeader = new ResponseHeader
            {
                Status = "S",
                Message = "get report form successful",
                Content = repairResult
            };
            return responseHeader;
        }
    }
}
