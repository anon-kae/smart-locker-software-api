using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IRepairFormService
    {
        ResponseHeader AddRepairForm(RepairFormDto repairFormDto);
        ResponseHeader GetRepairForm(int page, int perPage, RepairFormDto repairFormDto);
        ResponseHeader GetSummaryRepair(RepairFormDto repairFormDto);
        ResponseHeader ApproveRepairForm(RepairFormDto repairFormDto);
        ResponseHeader FinishRepairForm(RepairFormDto repairFormDto);
        ResponseHeader GetLockerRoomReadyToRepair(LockerRoomDto lockerRoomDto);


    }
}
