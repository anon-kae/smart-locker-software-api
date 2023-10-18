
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IIncomeDA
    {
        int GetIncome(DateTime date, string dateLength, int? locateId = null, int? lockerId = null, int? lkRoomId = null);
    }
}
