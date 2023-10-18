using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output.Dashboard
{
    public class LocationAndLockerResult
    {
        public int LocationAmount { get; set; }
        public int LocationNRAmount { get; set; }
        public int LockerAmount { get; set; }
        public int LockerNRAmount { get; set; }
        public int LockerRepairAmount { get; set; }
    }
}
