using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Booking
    {
        public Booking()
        {
            TransfersResBooking = new HashSet<Transfers>();
            TransfersTransBooking = new HashSet<Transfers>();
        }

        public int BookingId { get; set; }
        public int OverTime { get; set; }
        public int LkRoomId { get; set; }
        public int AccountId { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PassCode { get; set; }
        public int? RateTypeId { get; set; }

        public virtual LockerRooms LkRoom { get; set; }
        public virtual ICollection<Transfers> TransfersResBooking { get; set; }
        public virtual ICollection<Transfers> TransfersTransBooking { get; set; }
    }
}
