using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Dto
{
    public class StorageResourceInfo
    {
        public string ShareName { get; set; }
        public string Name { get; set; }

        public IDictionary<string,string> MetaData { get; set; }
    }
}
