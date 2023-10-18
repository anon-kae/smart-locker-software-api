using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Transactions
    {
        public string TransferId { get; set; }
        public string CardNumber { get; set; }
        public decimal Amont { get; set; }
        public int LkRoomId { get; set; }
        public DateTime CreateDate { get; set; }
        public int RateTypeId { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual RateTypes RateType { get; set; }
    }
}
