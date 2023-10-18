using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class RepairLockerRoom
    {
        public int RepairLkRoomId { get; set; }
        public int LkRoomId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int RepairFormId { get; set; }
        public string Remark { get; set; }

        public virtual RepairForm RepairForm { get; set; }
    }
}
