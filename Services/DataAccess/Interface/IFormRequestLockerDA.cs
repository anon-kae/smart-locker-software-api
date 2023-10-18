using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IFormRequestLockerDA
    {
        PageModel<FormRequestDto> GetAllFormRequestByAccountId(int page, int perPage, int accountId , string keyWord);
        int GetRoomAmountByLkSizeIdAndFormId(int lkSizeId, int formId);

        PageModel<FormRequestDto> SearchFormRequest(int page, int perPage, FormRequestDto keyWord);
    }
}
