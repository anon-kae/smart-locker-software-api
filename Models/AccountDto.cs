using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string CreateDate { get; set; }
        public string Reason { get; set; }
        public string Search { get; set; }
        public string Email { get; set; }
        public string PartnerType { get; set; }
        public string PartnerNumber { get; set; }
        public string PartnerAddress { get; set; }
        public string TelNo { get; set; }
    }
}
