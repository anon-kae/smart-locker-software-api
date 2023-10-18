using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Transfers
    {
        public int TransferId { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int TransBookingId { get; set; }
        public int ResBookingId { get; set; }

        public virtual Booking ResBooking { get; set; }
        public virtual Booking TransBooking { get; set; }
    }
}
