using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Common
{
    public class YrsUnAuthorizedException : Exception
    {
        public YrsUnAuthorizedException() : base() { }

        public YrsUnAuthorizedException(string message) : base(message) { }

        public YrsUnAuthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
