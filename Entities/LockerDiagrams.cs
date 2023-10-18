using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class LockerDiagrams
    {
        public int LkDiagramId { get; set; }
        public int ColumnPosition { get; set; }
        public int RowPosition { get; set; }
        public int LkRoomId { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual LockerRooms LkRoom { get; set; }
    }
}
