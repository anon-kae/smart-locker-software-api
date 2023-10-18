using System;

namespace smartlocker.software.api.Models
{
    public class NotificationDto
    {
        public int? NotiId { get; set; }
        public string Description { get; set; }
        public string ReadStatus { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int EventId { get; set; }
        public int AccountId { get; set; }
        public string EventName { get; set; }

    }
}