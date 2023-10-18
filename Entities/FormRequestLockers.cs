using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class FormRequestLockers
    {
        public FormRequestLockers()
        {
            LockerAmount = new HashSet<LockerAmount>();
        }

        public int FormId { get; set; }
        public int AccountId { get; set; }
        public int LockerId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FormCode { get; set; }
        public string Remark { get; set; }
        public string OptionalRequest { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Lockers Locker { get; set; }
        public virtual ICollection<LockerAmount> LockerAmount { get; set; }
    }
}
