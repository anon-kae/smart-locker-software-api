using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IFormRequestLockerService
    {
        ResponseHeader AddFormRequestLocker(FormRequestDto formRequestDto);
        ResponseHeader GetAllFormRequestByAccountId(int page, int perPage, int accountId ,string keyWord);
        ResponseHeader SearchFormRequest(int page, int perPage, FormRequestDto keyWord);
        ResponseHeader ApproveForm(BaseUpdateDto baseUpdateDto);
        ResponseHeader ApproveFormAndInstallLocker(BaseUpdateDto baseUpdateDto);
        ResponseHeader CreateLockerDiagramByFormId(FormCreateDiagramDto formCreateDiagram);
    }
}
