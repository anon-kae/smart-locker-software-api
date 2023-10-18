using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class BookingUpdateDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
    }
}
