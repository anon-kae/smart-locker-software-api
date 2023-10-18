using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IAccountDA
    {
        Accounts GetAccountByEmail(string email);
        Accounts GetAccountByUserNameAndPasswordAndRoleId(string username, string password, int roleId);
        PageModel<AccountDto> GetPartnerAccount(int page, int perPage, AccountDto accountDto);
        List<AccountDto> CountAccount();

    }
}
