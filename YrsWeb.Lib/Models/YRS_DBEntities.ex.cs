using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
    public partial class YRS_DBEntities
    {
        public YRS_DBEntities(string envPrefix)
    : base(String.Format("name={0}_YRS_DBEntities", envPrefix)) { }
    }
}
