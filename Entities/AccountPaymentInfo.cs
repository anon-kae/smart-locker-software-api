using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class AccountPaymentInfo
    {
        public int InfoId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public int CardTypeId { get; set; }
        public DateTime ExpDate { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual CardType CardType { get; set; }
    }
}
