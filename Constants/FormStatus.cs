using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Constants
{
    public class FormStatus
    {
        public const string ValidateForm = "VF";
        public const string RejectForm = "RF";
        public const string WaitforDiagram = "WD";
        public const string ValidateDiagram = "VD";
        public const string RejectDiagram = "RD";
        public const string WaitforPayment = "WP";
        public const string WaitforActivate = "WA";
        public const string Activate = "A";

    }
}
