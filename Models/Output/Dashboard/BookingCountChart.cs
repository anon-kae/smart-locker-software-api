using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output.Dashboard
{
    public class BookingCountChart
    {
        public string LocateName { get; set; }
        public int BookingCount { get; set; }

        //for booking by locker only
        public string LockerCode { get; set; }
    }
}
