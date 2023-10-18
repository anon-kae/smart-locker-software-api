using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class TransferDetailDto
    {
        public int BookingId { get; set; }
        public int AccountId { get; set; }
    }
}
