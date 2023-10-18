using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Accounts
    {
        public Accounts()
        {
            AccountPaymentInfo = new HashSet<AccountPaymentInfo>();
            FormRequestLockers = new HashSet<FormRequestLockers>();
            Locations = new HashSet<Locations>();
            Notifications = new HashSet<Notifications>();
            PartnerDetail = new HashSet<PartnerDetail>();
            RepairForm = new HashSet<RepairForm>();
            Transactions = new HashSet<Transactions>();
        }

        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IdCard { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int RoleId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<AccountPaymentInfo> AccountPaymentInfo { get; set; }
        public virtual ICollection<FormRequestLockers> FormRequestLockers { get; set; }
        public virtual ICollection<Locations> Locations { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<PartnerDetail> PartnerDetail { get; set; }
        public virtual ICollection<RepairForm> RepairForm { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
