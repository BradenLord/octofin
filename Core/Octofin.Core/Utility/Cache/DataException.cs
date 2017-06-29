using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Utility.Cache
{
    class DataException : Exception
    {
        public DataException() : base() { }
        public DataException(string message) : base(message) { }
        public DataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
