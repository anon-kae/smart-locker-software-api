using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class BookingDto
    {
        public int? BookingId { get; set; }
        public int? OverTime { get; set; }
        public int LkRoomId { get; set; }
        
        public int AccountId { get; set; }
        public int RateTypeId { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PassCode { get; set; }
        public string LocateName { get; set; }
        public string LkRoomCode { get; set; }

        
       
    }
}
