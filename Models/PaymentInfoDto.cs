using SmartLocker.Software.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class PaymentInfoDto
    {
        public int? InfoId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public int CardTypeId { get; set; }
        public string CVV { get; set; }
        public DateTime ExpDate { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int AccountId { get; set; }
        public long? Amount { get; set; }
        public int? RateTypeId { get; set; }

        public int? LkRoomId { get; set; }
    }
}
