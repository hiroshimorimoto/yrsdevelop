using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Dto
{
    public class PlanListSearchEt
    {
        public List<string> AcceptDates { get; set; }

        public List<int> CategoryIds { get; set; }

        public List<int> AreaIds { get; set; }


        public string AcceptDateFrom{ get; set; }

        public string AcceptDateTo { get; set; }

        public Nullable<short> MaxApplicantsNum { get; set; }

    }
}
