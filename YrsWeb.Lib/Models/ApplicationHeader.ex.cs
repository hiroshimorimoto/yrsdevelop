using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
	public partial class ApplicationHeader
	{
		public string InsertDateStr => this.InsertDate.ToString("yyyy/MM/dd HH:mm");

		public int SumNum => this.Num_Adult + this.Num_Child + this.Num_Infant;

		public string TransportationTitle
		{
			get
			{
				switch (this.Transportation)
				{
					case 0:
						return "公共交通機関";
					case 1:
						return "車・バイク";
					default:
						return "";
				}
			}
		}

		public string PaymentMethodTitle
		{
			get
			{
				/*"0：クレジットカード払い
				1：銀行振込
				2：現地支払"
				*/
				switch (this.PaymentMethods)
				{
					case 0:
						return "クレジットカード払い";
					case 1:
						return "銀行振込";
					case 2:
						return "現地支払";
					default:
						return "";
				}
			}
		}
	}
}
