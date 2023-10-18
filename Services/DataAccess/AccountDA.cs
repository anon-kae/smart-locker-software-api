using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
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
    public class AccountDA : IAccountDA
    {
        private readonly IBaseRepository baseRepository;
        private readonly IBaseService baseService;
        public AccountDA(IBaseRepository baseRepository, IBaseService baseService)
        {
            this.baseRepository = baseRepository;
            this.baseService = baseService;
        }

        public List<AccountDto> CountAccount()
        {
            string query = $@"SELECT * FROM ACCOUNTS WHERE Status = 'W'";
            List<AccountDto> accountResult = baseRepository.QueryString<AccountDto>(query).ToList();
            return accountResult;
        }

        public Accounts GetAccountByEmail(string email)
        {
            string query = $@" SELECT * FROM ACCOUNTS INNER JOIN ROLES ON ROLES.RoleId = ACCOUNTS.RoleId
                                WHERE Email = '{email}' AND ACCOUNTS.Status = 'A' AND ROLES.RoleName = 'User' ";
            var accountResult = baseRepository.QueryString<Accounts>(query).FirstOrDefault();
            return accountResult;
        }

        public Accounts GetAccountByUserNameAndPasswordAndRoleId(string username, string password, int roleId)
        {
            string query = $@" SELECT * FROM ACCOUNTS 
                                WHERE Email = '{username}' AND RoleId = {roleId} AND Status = 'A' ";

            Accounts accountResult = baseRepository.QueryString<Accounts>(query).FirstOrDefault();
            if(accountResult == null)
            {
                throw new UnauthorizedAccessException("Your username or password incorrect try again.");
            }
            var res = EncryptionUtilities.IsPasswordValid(password, accountResult.Password);
            if (res == true)
                return accountResult;
            else
                return null;
        }

        public PageModel<AccountDto> GetPartnerAccount(int page, int perPage, AccountDto accountDto)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($@" SELECT A.*,R.RoleName,RD.* FROM ACCOUNTS A
                                INNER JOIN ROLES R ON A.RoleId = R.RoleId
                                LEFT JOIN PARTNER_DETAIL RD ON A.AccountId = RD.AccountId
                                WHERE (
                                FirstName  LIKE '%{accountDto.FirstName}%' 
                                AND LastName  LIKE '%{accountDto.LastName}%' 
                                AND R.RoleName = 'Partner' ");
            if (!String.IsNullOrEmpty(accountDto.Status))
            {
                sql.Append($@"AND A.Status  = '{accountDto.Status}'  ");
            }
            sql.Append(" ) ORDER BY A.CreateDate DESC  ");

            PageModel<AccountDto> pageModel = baseService.Pagination<AccountDto>(page, perPage, sql.ToString());
            return pageModel;
        }
    }
}
