using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class LockerDto
    {
        public int LockerId { get; set; }
        public string LockerCode { get; set; }
        public int LocateId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? AccountId { get; set; }

    }
}
