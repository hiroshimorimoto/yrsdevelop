using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
	public partial class ApplicationPlan
	{
		public int GetSumFee(short num_Adult, short num_Child, short num_Infant)
		{
			var ret = 0;
			ret += this.Fee_Adult * num_Adult;
			ret += this.Fee_Child * num_Child;
			ret += this.Fee_Infant * num_Infant;
			return ret;
		}

		public int GetSumFee(ApplicationHeader header)
		{
			return this.GetSumFee(header.Num_Adult, header.Num_Child, header.Num_Infant);
		}
	}
}
