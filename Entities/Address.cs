using System;
using System.Collections.Generic;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class Address
    {
        public Address()
        {
            Locations = new HashSet<Locations>();
        }

        public int AddressId { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string District { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SubDistrict { get; set; }

        public virtual ICollection<Locations> Locations { get; set; }
    }
}
