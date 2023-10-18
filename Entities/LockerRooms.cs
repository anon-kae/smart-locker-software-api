using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class LockerRooms
    {
        public LockerRooms()
        {
            Booking = new HashSet<Booking>();
            LockerDiagrams = new HashSet<LockerDiagrams>();
        }

        public int LkRoomId { get; set; }
        public string LkRoomCode { get; set; }
        public int LockerId { get; set; }
        public int LkSizeId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual LockerSizes LkSize { get; set; }
        public virtual Lockers Locker { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<LockerDiagrams> LockerDiagrams { get; set; }
    }
}
