using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class IncomeLockerDto
    {
        public List<IncomeLocker> IncomeLockers { get; set; }
        public string[] RangeTitle { get; set; }
        [JsonIgnore]
        public LockerDto LockerFilter { get; set; }

        [JsonIgnore]
        public string DurationType { get; set; }
        [JsonIgnore]
        public DateTime? StartDate { get; set; }
        [JsonIgnore]
        public DateTime? EndDate { get; set; }
    }

    public class IncomeLocker
    {
        public int LockerId { get; set; }
        public string LockerCode { get; set; }
        public double[] IncomeResult { get; set; }
    }
}
