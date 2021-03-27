//using Microsoft.Identity.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
    public class ProviderPostEt
    {

        public EditModes EditModeType
        {
            get
            {
                return (EditModes)Enum.Parse(typeof(EditModes), this.EditMode, true);
            }
            set
            {
                if(Enum.TryParse<EditModes>(value.ToString(), out EditModes newValue))
                {
                    this.EditMode = newValue.ToString();
                }
            }
        }

        public string EditMode { get; set; }

        public bool IsSendRegistMail { get; set; }

        public Provider_M ProviderEt { get; set; }
    }
}
