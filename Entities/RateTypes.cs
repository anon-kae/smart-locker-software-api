using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class RateTypes
    {
        public RateTypes()
        {
            ServiceRate = new HashSet<ServiceRate>();
            Transactions = new HashSet<Transactions>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<ServiceRate> ServiceRate { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
