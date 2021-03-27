using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
	public class PublicPlanModel
	{
		public PublicPlanInfo PlanInfo { get; set; }

		public List<string> AcceptDates { get; set; }

		public List<DateFeeInfo> DateFeeList { get; set; }

		public List<int> CategoryIds { get; set; }

		public List<int> AreaIds { get; set; }

		public PlanImage TopImage { get; set; }

		public List<PlanImage> TempImages { get; set; }

		public class DateFeeInfo
		{
			public DateFeeInfo() { }
			public DateFeeInfo(PlanDate planDate)
			{
				this.AcceptDate = planDate.AcceptDate.ToString("yyyy/MM/dd");
				this.Fee_Adult = planDate.Fee_Adult;
				this.Fee_Child = planDate.Fee_Child;
				this.Fee_Infant = planDate.Fee_Infant;
			}


			public string AcceptDate { get; set; }
			public Nullable<int> Fee_Adult { get; set; }
			public Nullable<int> Fee_Child { get; set; }
			public Nullable<int> Fee_Infant { get; set; }
		}
	}
}
