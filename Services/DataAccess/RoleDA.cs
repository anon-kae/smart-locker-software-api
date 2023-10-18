using SmartLocker.Software.Backend.Entities;
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
    public class RoleDA : IRoleDA
    {
        private readonly IBaseRepository baseRepository;
        public RoleDA(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public Roles GetRoleByRoleName(string roleName)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@" SELECT * FROM ROLES ");
            query.Append($@" WHERE RoleName = '{roleName}' ");

            var accountResult = baseRepository.QueryString<Roles>(query.ToString()).FirstOrDefault();
            return accountResult;
        }
    }
}
