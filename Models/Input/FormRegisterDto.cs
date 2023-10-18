using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class FormRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IdCard { get; set; }
        public string RoleUser { get; set; }
        public string TelNo { get; set; }

        public string PartnerType { get; set; }

        public string PartnerNumber { get; set; }

        public string PartnerAddress { get; set; }

    }
}
