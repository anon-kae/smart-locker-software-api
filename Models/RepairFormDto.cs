using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class RepairFormDto
    {
        public int RepairFormId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Remark { get; set; }
        public int AccountId { get; set; }
        public List<RepairLkRoomDto> RepairLkRoomList { get; set; }

        public List<int> LkRoomIdList { get; set; }
        public string LockerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
