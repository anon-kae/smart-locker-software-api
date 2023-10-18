using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class PartnerDetail
    {
        public int PartnerDetailId { get; set; }
        public string PartnerType { get; set; }
        public string PartnerNumber { get; set; }
        public string PartnerAddress { get; set; }
        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public string TelNo { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
