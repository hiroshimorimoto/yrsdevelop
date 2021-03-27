using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Common;

namespace YrsWeb.Lib.Models
{
	public partial class PublicPlanInfo
	{
		/// <summary>
		/// パブリックイメージパス
		/// </summary>
		//public string PublicTopImageUrl=> String.Format("/imgs/plans/{0}/top", this.PlanId);
		public string PublicTopImageUrl
		{
			get
			{
				if (this.TopImageNo.HasValue)
				{
					return String.Format("/imgs/plans/{0}/top", this.PlanId);
				}
				else
				{
					return null;
				}
			}
		}

		public string PlanPageUrl => String.Format(@"{0}tour_info/?pid={1}", Constants.YRS_SYSTEM_URL, this.PlanId);
	}
}
