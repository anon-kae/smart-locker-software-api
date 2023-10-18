using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class BaseService : IBaseService
    {
        private readonly IBaseRepository baseRepository;
        public BaseService(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public PageModel<T> Pagination<T>(int page, int perPage, string sql)
        {
            var pageReal= (page - 1) * perPage;
            StringBuilder sqlResult = new StringBuilder(sql);
            sqlResult.Append($@" OFFSET {pageReal} ROWS FETCH NEXT {perPage} ROWS ONLY  ");

            PageModel<T> pageResult = new PageModel<T>
            {
                Content = baseRepository.QueryString<T>(sqlResult.ToString()).ToList(),
                Page = page,
                PerPage = perPage,
                TotalElement = baseRepository.QueryString<T>(sql).ToList().Count
            };
            return pageResult;
        }

        public string CheckAccountRoleByAccountId(int AccountId)
        {
            string sqlAccount = $@"SELECT [AccountId]
                                    ,[AccountCode]
                                    ,[FirstName]
                                    ,[LastName]
                                    ,[Email]
                                    ,[Password]
                                    ,[IdCard]
                                    ,[Status]
                                    ,[CreateDate]
                                    ,[UpdateDate]
                                    ,[ApproveDate]
                                    ,[RoleId]
                                FROM [SmartLocker].[dbo].[ACCOUNTS]
	                            WHERE [AccountId] = {AccountId}";
            var accountResult = baseRepository.QueryString<Accounts>(sqlAccount).FirstOrDefault();
            if (accountResult == null)
            {
                return "400";
            }
            else
            {
                string sqlRole = $@"SELECT [RoleId]
                                          ,[RoleName]
                                          ,[Status]
                                          ,[CreateDate]
                                          ,[UpdateDate]
                                      FROM [SmartLocker].[dbo].[ROLES]
                                      WHERE [RoleId] = {accountResult.RoleId}";
                var roleResult = baseRepository.QueryString<Roles>(sqlRole).FirstOrDefault();
                if (roleResult == null)
                {
                    return "409";
                }
                else
                {
                    return roleResult.RoleName;
                }
            }
        }

        public string[] WeekLabel(DateTime? dateTime = null)
        {
            string[] label = new string[7];
            DateTime realDateTime = dateTime == null ? DateTime.Now : dateTime.Value;

            for (int i = 6; i >= 0; i--)
            {
                label[i] = realDateTime.ToString("dd MMMM yyyy", new CultureInfo("th-TH"));
                realDateTime = realDateTime.AddDays(-1);
            }
            return label;
        }
        public string[] MonthLabel(DateTime? dateTime = null)
        {
            string[] label = new string[7];
            DateTime realDateTime = dateTime == null ? DateTime.Now : dateTime.Value;
            for (int i = 6; i >= 0; i--)
            {
                label[i] = realDateTime.ToString("MMMM yyyy", new CultureInfo("th-TH"));
                realDateTime = realDateTime.AddMonths(-1);
            }
            return label;
        }
        public string[] YearLabel(DateTime? dateTime = null)
        {
            string[] label = new string[7];
            DateTime realDateTime = dateTime == null ? DateTime.Now : dateTime.Value;
            for (int i = 6; i >= 0; i--)
            {
                label[i] = realDateTime.ToString("yyyy", new CultureInfo("th-TH"));
                realDateTime = realDateTime.AddYears(-1);
            }
            return label;
        }

    }
}
