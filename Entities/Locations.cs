using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Locations
    {
        public Locations()
        {
            Lockers = new HashSet<Lockers>();
        }

        public int LocateId { get; set; }
        public string LocateName { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public int AddressId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int AccountId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Remark { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Lockers> Lockers { get; set; }
    }
}
