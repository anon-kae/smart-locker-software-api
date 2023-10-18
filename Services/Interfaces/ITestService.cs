using System.Collections.Generic;
using SmartLocker.Software.API.Models;

namespace SmartLocker.Software.API.Services.Interfaces
{
    public interface ITestService
    {
        List<RoleDto> GetRole();
    }
}