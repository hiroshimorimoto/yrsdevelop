using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Common
{
    public class YrsWebException: Exception
    {
        public YrsWebException() : base() { }

        public YrsWebException(string message) : base(message) { }

        public YrsWebException(string message, Exception innerException) : base(message, innerException) { }
    }
}
