using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
    public partial class Provider_M
    {
        public enum RegistStatusTypes
        {
            PreRegist = (byte)0,
            WaitProRegist = (byte)1,
            Registed = (byte)2
        }

        public RegistStatusTypes RegistStatusType
        {
            get
            {
                return (RegistStatusTypes)this.RegistStatus;
            }
            set
            {
                this.RegistStatus = (byte)value;
            }
        }
    }
}
