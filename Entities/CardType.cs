using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class CardType
    {
        public CardType()
        {
            AccountPaymentInfo = new HashSet<AccountPaymentInfo>();
        }

        public int CardTypeId { get; set; }
        public string TypeCardName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CardTypeImgUrl { get; set; }

        public virtual ICollection<AccountPaymentInfo> AccountPaymentInfo { get; set; }
    }
}
