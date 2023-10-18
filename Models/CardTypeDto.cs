using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models
{
    public class CardTypeDto
    {
        public int CardTypeId { get; set; }
        public string TypeCardName { get; set; }
        public string Status { get; set; }
    }
}
