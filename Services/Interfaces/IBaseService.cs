using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IBaseService
    {
        PageModel<T> Pagination<T>(int page , int perPage , string sql );
        string CheckAccountRoleByAccountId(int AccountId);
        string[] WeekLabel(DateTime? dateTime = null);
        string[] MonthLabel(DateTime? dateTime = null);
        string[] YearLabel(DateTime? dateTime = null);
    }
}
