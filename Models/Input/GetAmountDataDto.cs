using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class GetAmountDataDto
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
