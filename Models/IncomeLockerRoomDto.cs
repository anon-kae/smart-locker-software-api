using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class IncomeLockerRoomDto
    {
        public List<IncomeLockerRoom> IncomeLockerRooms { get; set; }
        public string[] RangeTitle { get; set; }
        [JsonIgnore]
        public int LockerId { get; set; }
        [JsonIgnore]
        public int? AccountId { get; set; }
        [JsonIgnore]
        public string DurationType { get; set; }
        [JsonIgnore]
        public DateTime? StartDate { get; set; }
        [JsonIgnore]
        public DateTime? EndDate { get; set; }
    }

    public class IncomeLockerRoom
    {
        public string LockerRoomCode { get; set; }
        public double[] IncomeResult { get; set; }
    }
}
