using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class BaseUpdateDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
    }
}
