using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IRepairFormDA
    {
        PageModel<RepairFormDto> GetRepairForm(int page, int perPage, RepairFormDto repairFormDto);
        List<RepairLkRoomDto> GetRepairLkRoomList(int repairFormId);
        List<int> GetSummaryRepair(RepairFormDto repairFormDto);
    }
}
