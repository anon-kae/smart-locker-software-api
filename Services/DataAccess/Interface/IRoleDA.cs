using SmartLocker.Software.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface IRoleDA
    {
        Roles GetRoleByRoleName(String roleName);
    }
}
