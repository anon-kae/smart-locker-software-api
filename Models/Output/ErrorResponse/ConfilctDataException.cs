    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output.ErrorResponse
{
    [Serializable]
    class ConfilctDataException : Exception
    {
        public ConfilctDataException()
        {

        }
        public ConfilctDataException(string message) : base(String.Format(message))
        {

        }
    }
}
