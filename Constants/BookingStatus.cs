using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Constants
{
    public class BookingStatus
    {
        public const string WaitProcess = "WP";
        public const string Process = "P";
        public const string WaitFinish = "WF";
        public const string Finish = "F";
        public const string Cancel = "C";
        public const string WaitTranfer = "WT";
        public const string WaitReceiver = "WR";
        public const string Tranfer = "T";
        public const string Receiver = "R";
        public const string NonReceiver = "NR";
        public const string Delete = "D";
    }
}
