using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class ServiceChargeDto
    {
        public int Srid { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
        public string Status { get; set; }
        public string TypeName { get; set; }
        public string LkSizeName { get; set; }
        public int LkSizeId { get; set; }

        public string LkRoomCode { get; set; }
        public decimal Price { get; set; }
        public decimal SurplusPrice { get; set; }

    }
}
