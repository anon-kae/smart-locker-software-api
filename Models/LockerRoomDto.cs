using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class LockerRoomDto
    {
        public int LkRoomId { get; set; }
        public string LkRoomCode { get; set; }
        public int LockerId { get; set; }
        public int LkSizeId { get; set; }
        public string LkSizeName { get; set; }

        public int ColumnPosition { get; set; }
        public int RowPosition { get; set; }


        public string Status { get; set; }
    }
}
