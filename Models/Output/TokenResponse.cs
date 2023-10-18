using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }

        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string FirstName { get;set; }
        public string LastName { get;set; }
    }
}
