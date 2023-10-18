using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IAccountService
    {
        ResponseHeader LoginAccount(FormLoginDto formLoginDto);
        Task<ResponseHeader> RegisterAccountAsync(FormRegisterDto formRegister);
        ResponseHeader VerifyAccount(string email, int id);
        ResponseHeader GetAccountByAccountCode(string accountCode);
        ResponseHeader ValidateDuplicateEmail(string email);
        ResponseHeader ValidateDuplicateEmailPartner(string email);

        ResponseHeader UpdateAccount(AccountDto accountDto);
        ResponseHeader GetPartnerAccount(int page, int perPage, AccountDto account);

        Task<ResponseHeader> ApproveAccount(AccountDto accountDto);


    }
}
