using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class ServiceRate
    {
        public int Srid { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int TypeId { get; set; }
        public int LkSizeId { get; set; }
        public decimal Price { get; set; }
        public decimal SurplusPrice { get; set; }

        public virtual LockerSizes LkSize { get; set; }
        public virtual RateTypes Type { get; set; }
    }
}
