using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output.Dashboard
{
    public class UserDashboard
    {
        public int FinishUsing { get; set; }
        public int TotalTransfer { get; set; }
        public int SuccessTransfer { get; set; }
        public int RejectTransfer { get; set; }
    }
}
