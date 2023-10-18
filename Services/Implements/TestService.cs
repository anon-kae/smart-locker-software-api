using System.Collections.Generic;
using SmartLocker.Software.API.Models;
using SmartLocker.Software.API.Services.Interfaces;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using System.Linq;


namespace SmartLocker.Software.API.Services.Implements
{
    public class TestService : ITestService
    {
        private readonly IBaseRepository _db;

        public TestService(IBaseRepository baseRepository)
        {
            this._db = baseRepository;
        }

        public List<RoleDto> GetRole()
        {
            string queryString = $@"SELECT * FROM ROLES";
            var result = _db.QueryString<RoleDto>(queryString).ToList();
            return result;
        }
    }
}