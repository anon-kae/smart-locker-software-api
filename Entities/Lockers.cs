using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Lockers
    {
        public Lockers()
        {
            FormRequestLockers = new HashSet<FormRequestLockers>();
            LockerRooms = new HashSet<LockerRooms>();
        }

        public int LockerId { get; set; }
        public string LockerCode { get; set; }
        public int LocateId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Locations Locate { get; set; }
        public virtual ICollection<FormRequestLockers> FormRequestLockers { get; set; }
        public virtual ICollection<LockerRooms> LockerRooms { get; set; }
    }
}
