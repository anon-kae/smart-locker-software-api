using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class LockerSizes
    {
        public LockerSizes()
        {
            LockerAmount = new HashSet<LockerAmount>();
            LockerRooms = new HashSet<LockerRooms>();
            ServiceRate = new HashSet<ServiceRate>();
        }

        public int LkSizeId { get; set; }
        public string LkSizeName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<LockerAmount> LockerAmount { get; set; }
        public virtual ICollection<LockerRooms> LockerRooms { get; set; }
        public virtual ICollection<ServiceRate> ServiceRate { get; set; }
    }
}
