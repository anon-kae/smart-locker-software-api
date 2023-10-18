using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output
{
    public class PageModel<T>
    {
        public List<T> Content{ get; set; } 
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalElement { get; set; }
    }
}
