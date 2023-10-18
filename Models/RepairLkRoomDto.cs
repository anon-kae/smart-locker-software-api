using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class RepairLkRoomDto
    {
        public int RepairLkRoomId { get; set; }
        public int LkRoomId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int RepairFormId { get; set; }
        public string Remark { get; set; }

        public string LkRoomCode { get; set; }
    }
}
