using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
	public class AppEditModel
	{
		public ApplicationHeader ApplicationHeader { get; set; }

		public List<ApplicationPlan> ApplicationPlans { get; set; } = new List<ApplicationPlan>();

		public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

		public List<ApplicationPayment> ApplicationPayments { get; set; } = new List<ApplicationPayment>();

		public List<PublicPlanModel> PlanModelList { get; set; } = new List<PublicPlanModel>();


		private bool? enableBankPay = null;

		/// <summary>
		/// 申込の合計
		/// </summary>
		/// <returns></returns>
		public int GetSumFee(){
			var ret = 0;

			foreach(ApplicationPlan appPlan in this.ApplicationPlans){
				ret += appPlan.GetSumFee(this.ApplicationHeader);
			}

			return ret;
		}

		/// <summary>
		/// 銀行振込 可・不可 フラグ
		/// </summary>
		public bool EnableBankPay
		{
			get
			{
				if (!this.enableBankPay.HasValue)
				{
					DateTime minAcceptDate = this.ApplicationPlans.Min(e => e.AcceptDate);
					minAcceptDate = DateTime.Parse(minAcceptDate.ToString("yyyy/MM/dd"));
					DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
					if (today.AddDays(7) <= minAcceptDate)
					{
						this.enableBankPay = true;
					}else{
						this.enableBankPay = false;
					}
				}

				return this.enableBankPay.Value;
			}
		}


		private bool? isAllPerson = null;
		/// <summary>
		/// 全 個人情報必要フラグ
		/// </summary>
		public bool IsAllPerson
		{
			get
			{
				if (!isAllPerson.HasValue)
				{
					//全 個人情報 必要フラグ
					this.isAllPerson = this.PlanModelList.Any(e => e.PlanInfo.AllPersonFlg.HasValue && e.PlanInfo.AllPersonFlg.Value);
				}

				return this.isAllPerson.Value;
			}
		}


		private short? maxApplicantsNum = null;
		public int MaxApplicantsNum
		{
			get
			{
				if (!this.maxApplicantsNum.HasValue)
				{
					this.maxApplicantsNum = 0;
					if (this.PlanModelList.Any(e => e.PlanInfo.MaxApplicantsNum.HasValue && e.PlanInfo.MaxApplicantsNum.Value > 0))
					{
						//最大申込人数
						List<short> numList = this.PlanModelList.Where(e => e.PlanInfo.MaxApplicantsNum.HasValue && e.PlanInfo.MaxApplicantsNum.Value > 0).Select(e => e.PlanInfo.MaxApplicantsNum.Value).ToList();
						if (numList != null && numList.Count > 0)
						{
							this.maxApplicantsNum = numList.Min();
						}
					}

				}
				return this.maxApplicantsNum.Value;

			}
		}
	}
}


