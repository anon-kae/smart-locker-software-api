using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class EventTypeDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
