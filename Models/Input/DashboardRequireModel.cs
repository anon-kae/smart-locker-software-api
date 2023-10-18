using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class DashboardRequireModel
    {
        public int AccountId { get; set; }
        public string RangeGraphType { get; set; }
        public int? MonthRange { get; set; }
    }
}
