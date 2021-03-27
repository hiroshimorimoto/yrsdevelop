using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace YrsWeb.Lib.Models
{
	public partial class ApplicationPayment
	{

		public ApplicationPayment() { }

		private HttpRequest _req;

		public ApplicationPayment(HttpRequest request)
		{
			this._req = request;

			this.ShopID = this.GetValue("ShopID");
			this.JobCd = this.GetValue("JobCd");

			if (int.TryParse(this.GetValue("Amount"), out int _amount))
			{
				this.Amount = _amount;
			}
			else
			{
				this.Amount = 0;
			}

			if (int.TryParse(this.GetValue("Tax"), out int _tax))
			{
				this.Tax = _tax;
			}
			else
			{
				this.Tax = 0;
			}

			this.Currency = this.GetValue("Currency");
			this.AccessID = this.GetValue("AccessID");
			this.AccessPass = this.GetValue("AccessPass");
			this.OrderID = this.GetValue("OrderID");
			this.TranID = this.GetValue("TranID");
			this.TranDate = this.GetValue("TranDate");
			this.CheckString = this.GetValue("CheckString");
			this.ErrCode = this.GetValue("ErrCode");
			this.ErrInfo = this.GetValue("ErrInfo");
			this.ClientField1 = this.GetValue("ClientField1");
			this.ClientField2 = this.GetValue("ClientField2");
			this.ClientField3 = this.GetValue("ClientField3");

			if (int.TryParse(this.ClientField1, out int _applicationId))
			{
				this.ApplicationId = _applicationId;
			}
			else
			{
				throw new YrsWeb.Lib.Common.YrsWebException("決済結果から申込IDが取得できません");
			}
		}

		private string GetValue(string fieldName, int index = 0)
		{
			return this._req.Form[fieldName][index];
		}
	}
}
