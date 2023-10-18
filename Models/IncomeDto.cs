using Newtonsoft.Json;
using smartlocker.software.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class IncomeDto
    {

        public List<Income> Incomes { get; set; }
        public string[] RangeTitle { get; set; }

        [JsonIgnore]
        public LocationDto LocationDto { get; set; }
        [JsonIgnore]
        public string DurationType { get; set; }
        [JsonIgnore]
        public DateTime? StartDate { get; set; }
        [JsonIgnore]
        public DateTime? EndDate { get; set; }
    }

    public class Income
    {
        public int LocateId { get; set; }
        public string LocateName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double[] IncomeResult { get; set; }
    }
}
