using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class PartnerTransaction
    {
        public int PtransferId { get; set; }
        public string TranferImgUrl { get; set; }
        public DateTime TranferDate { get; set; }
        public decimal TranferAmount { get; set; }
        public string TranferBank { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int AccountId { get; set; }
        public int FormId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual FormRequestLockers Form { get; set; }
    }
}
