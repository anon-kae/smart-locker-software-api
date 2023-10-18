using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class FormRequestDto
    {
        public int FormId { get; set; }
        public int AccountId { get; set; }
        public int SAmount { get; set; }
        public int MAmount { get; set; }
        public int LAmount { get; set; }
        public int LocateId { get; set; }

        public string Remark { get; set; }
        public string OptionalRequest { get; set; }

        public int LockerId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FormCode { get; set; }

        public string LockerCode { get; set; }
        public string LocateName { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }

        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
