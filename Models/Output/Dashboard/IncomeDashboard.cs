using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output.Dashboard
{
    public class IncomeDashboard
    {
        public string[] Label { get; set; }
        public string[] RateTypeName { get; set; }
        public Decimal[][] Value { get; set; }
    }
}
