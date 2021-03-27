using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Dto
{
    public class StorageFileInfo:StorageResourceInfo
    {

        public long Size { get; set; }

        public string Extension => new System.IO.FileInfo(this.Name).Extension;

        public string ContentType { get; set; }

        public DateTime? LastModified { get; set; }


    }
}
