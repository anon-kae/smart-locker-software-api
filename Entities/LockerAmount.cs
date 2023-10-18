using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class LockerAmount
    {
        public int LkAmountId { get; set; }
        public int Amount { get; set; }
        public int FormId { get; set; }
        public int LkSizeId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual FormRequestLockers Form { get; set; }
        public virtual LockerSizes LkSize { get; set; }
    }
}
