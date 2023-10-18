using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class RepairForm
    {
        public RepairForm()
        {
            RepairLockerRoom = new HashSet<RepairLockerRoom>();
        }

        public int RepairFormId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Remark { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual ICollection<RepairLockerRoom> RepairLockerRoom { get; set; }
    }
}
