using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Dto
{
	public class AppEditModelRequest
	{


		public List<AppEditModelRequestItem> PlanIdList = new List<AppEditModelRequestItem>();
	}

	public class AppEditModelRequestItem
	{
		public int PlanId { get; set; }
		public string AcceptDate { get; set; }
	}
}
