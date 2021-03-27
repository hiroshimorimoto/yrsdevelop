using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
    public class PlanModel
    {
        public EditModes EditModeType
        {
            get
            {
                return (EditModes)Enum.Parse(typeof(EditModes), this.EditMode, true);
            }
            set
            {
                if (Enum.TryParse<EditModes>(value.ToString(), out EditModes newValue))
                {
                    this.EditMode = newValue.ToString();
                }
            }
        }

        public string EditMode { get; set; }

        public PlanInfo PlanEt { get; set; }

        public List<string> AcceptDates { get; set; }

        public List<PlanDate> DateFeeList{ get; set; }

        public List<int> SelectedCategoryIds { get;set; }

        public List<int> SelectedAreaIds { get; set; }

        public  PlanImage TopImage { get; set; }

        public string SelectedTopImageFileName { get; set; }

        public List<PlanImage> TempImages { get; set; }
    }
}
