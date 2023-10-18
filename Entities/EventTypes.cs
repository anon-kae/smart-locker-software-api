using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class EventTypes
    {
        public EventTypes()
        {
            Notifications = new HashSet<Notifications>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Notifications> Notifications { get; set; }
    }
}
