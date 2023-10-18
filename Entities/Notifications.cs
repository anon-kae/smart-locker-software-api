using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Notifications
    {
        public int NotiId { get; set; }
        public string Description { get; set; }
        public string ReadStatus { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int EventId { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual EventTypes Event { get; set; }
    }
}
