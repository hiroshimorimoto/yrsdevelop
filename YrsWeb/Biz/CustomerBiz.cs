using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Azure.Storage.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;
using Microsoft.AspNetCore.Hosting;


namespace YrsWeb.Biz
{
	public class CustomerBiz : AbstractBiz
	{
		private IHostingEnvironment _hostingEnvironment;
		private FileBiz fileBiz;

		private SendAppCompMailBiz mailBiz = null;

		public CustomerBiz(BaseController controller, YRS_DBEntities dbContext, IHostingEnvironment hostingEnvironment) : base(controller, dbContext)
		{
			this.fileBiz = new FileBiz(controller, dbContext);
			this._hostingEnvironment = hostingEnvironment;
		}


		#region プランリスト関連
		public List<PublicPlanInfo> GetPlanList()
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			var q = base.DbContext.PublicPlanInfo.AsQueryable();
			q = q.OrderBy(e => e.PlanId);

			List<PublicPlanInfo> ret = q.ToList();

			return ret;
		}


		public List<PublicPlanInfo> FindPlanList(PlanListSearchEt searchEt)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			var q = base.DbContext.PublicPlanInfo.AsQueryable();

			List<int> targetPlanIdList = new List<int>();

			bool searchFlg = true;
			//カテゴリの絞り込み
			#region カテゴリの絞り込み
			if (searchFlg && searchEt.CategoryIds != null && searchEt.CategoryIds.Count > 0)
			{
				var subQ = base.DbContext.PlanCategory.AsQueryable();
				if (targetPlanIdList.Count > 0)
				{
					subQ = subQ.Where(e => targetPlanIdList.Contains(e.PlanId));
				}

				List<int> subList = subQ
			   .Where(e => searchEt.CategoryIds.Contains(e.CategoryId))
			   .Select(e => e.PlanId)
			   .Distinct()
			   .ToList();

				if (subList.Count <= 0)
				{
					searchFlg = false;
				}
				else
				{
					targetPlanIdList.AddRange(subList);
				}
			}
			#endregion

			//エリアの絞り込み
			#region MyRegion
			if (searchFlg && searchEt.AreaIds != null && searchEt.AreaIds.Count > 0)
			{
				var subQ = base.DbContext.PlanArea.AsQueryable();
				if (targetPlanIdList.Count > 0)
				{
					subQ = subQ.Where(e => targetPlanIdList.Contains(e.PlanId));
				}

				List<int> subList = subQ
			   .Where(e => searchEt.AreaIds.Contains(e.AreaId))
			   .Select(e => e.PlanId)
			   .Distinct()
			   .ToList();
				if (subList.Count <= 0)
				{
					searchFlg = false;
				}
				else
				{
					targetPlanIdList.AddRange(subList);
				}
			}
			#endregion

			//受付日の絞り込み
			#region 受付日の絞り込み
			if (searchEt.AcceptDates != null && searchEt.AcceptDates.Count > 0)
			{
				searchEt.AcceptDates = searchEt.AcceptDates.Distinct().ToList();

				List<string> seachDateList = new List<string>();
				foreach (string strDate in searchEt.AcceptDates)
				{
					if (DateTime.TryParse(strDate, out DateTime date))
					{
						seachDateList.Add(date.ToString("yyyy/MM/dd"));
					}
				}

				var subQ = base.DbContext.PlanDateStr.AsQueryable();
				if (targetPlanIdList.Count > 0)
				{
					subQ = subQ.Where(e => targetPlanIdList.Contains(e.PlanId));
				}

				List<int> subList = subQ
				.Where(e => seachDateList.Contains(e.AcceptDateStr))
				.Select(e => e.PlanId)
				.Distinct()
				.ToList();
				if (subList.Count <= 0)
				{
					searchFlg = false;
				}
				else
				{
					targetPlanIdList.AddRange(subList);
				}
			}
			#endregion

			if (searchFlg)
			{
				if (targetPlanIdList.Count > 0)
				{
					//重複を削除
					targetPlanIdList = targetPlanIdList.Distinct().ToList();
					q = q.Where(e => targetPlanIdList.Contains(e.PlanId));
				}
			}
			else
            {
				return new List<PublicPlanInfo>();
			}

			// 非表示フラグ
			#region 非表示フラグ
			q = q.Where(e => e.HiddenFlg == false);
			#endregion

			//受付可能日 From
			#region 受付可能日 From
			if (!String.IsNullOrEmpty(searchEt.AcceptDateFrom) && DateTime.TryParse(searchEt.AcceptDateFrom, out DateTime acceptDateFrom))
			{
				q = q.Where(e => acceptDateFrom <= e.MaxAcceptDate);
			}
			#endregion

			//受付可能日 To
			#region 受付可能日 To
			if (!String.IsNullOrEmpty(searchEt.AcceptDateTo) && DateTime.TryParse(searchEt.AcceptDateTo, out DateTime acceptDateTo))
			{
				q = q.Where(e => acceptDateTo >= e.MinAcceptDate);
			}
			#endregion

			//最大申込人数
			#region 最大申込人数
			if (searchEt.MaxApplicantsNum.HasValue)
			{
				q = q.Where(e => e.MaxApplicantsNum >= searchEt.MaxApplicantsNum);
			}
			#endregion

			//検索実行
			List<PublicPlanInfo> ret = null;
			q = q.OrderBy(e => e.PlanEndDate).ThenBy(e => e.PlanId);
			//q = q.OrderBy(e => e.PlanId);
			ret = q.ToList();

			return ret;
		}



		public PublicPlanModel GetPlanModel(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			if (planId <= 0)
			{
				throw new YrsWebException("プランIDが不正です");
			}

			PublicPlanModel ret = new PublicPlanModel();

			ret.PlanInfo = base.DbContext.PublicPlanInfo.SingleOrDefault(e => e.PlanId == planId);
			if (ret.PlanInfo == null)
			{
				return null;
			}

			//プラン受付日
			List<PlanDate> planDates = base.DbContext.PlanDate.Where(e => e.PlanId == planId).ToList();
			ret.AcceptDates = planDates.Select(e => e.AcceptDate.ToString("yyyy/MM/dd")).ToList();
			ret.DateFeeList = planDates.Where(e => e.Fee_Adult.HasValue || e.Fee_Child.HasValue || e.Fee_Infant.HasValue).Select(e => new PublicPlanModel.DateFeeInfo(e)).ToList();

			//カテゴリリスト
			ret.CategoryIds = base.DbContext.PlanCategory.Where(e => e.PlanId == planId).Select(e => e.CategoryId).ToList();

			//エリアリスト
			ret.AreaIds = base.DbContext.PlanArea.Where(e => e.PlanId == planId).Select(e => e.AreaId).ToList();

			//トップ画像
			PlanImage topImage = base.DbContext.PlanImage.Where(e => e.PlanId == planId && e.ImageLevel <= 0).FirstOrDefault();
			ret.TopImage = topImage;

			//その他画像
			List<PlanImage> tempImages = base.DbContext.PlanImage.Where(e => e.PlanId == planId && e.ImageLevel > 0).ToList();
			ret.TempImages = tempImages;

			return ret;
		}

		public List<PlanCategory> GetPlanCategoryList(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			List<PlanCategory> ret = new List<PlanCategory>();

			//カテゴリリスト
			ret = base.DbContext.PlanCategory.Where(e => e.PlanId == planId).ToList();

			return ret;
		}

		public List<int> GetPlanCategoryIdList(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			List<int> ret = new List<int>();

			//カテゴリリスト
			ret = base.DbContext.PlanCategory.Where(e => e.PlanId == planId).Select(e => e.CategoryId).ToList();

			return ret;
		}



		public List<PlanImage> GetPublicPlanImageList(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			if (planId <= 0)
			{
				throw new YrsWebException("プランIDが不正です");
			}

			PublicPlanInfo planInfo = base.DbContext.PublicPlanInfo.SingleOrDefault(e => e.PlanId == planId);
			if (planInfo == null)
			{
				return null;
			}

			List<PlanImage> ret = new List<PlanImage>();

			//トップ画像
			PlanImage topImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == planId && e.ImageLevel <= 0);
			if (topImage != null)
			{
				ret.Add(topImage);
			}

			//その他画像
			List<PlanImage> tempImages = base.DbContext.PlanImage.Where(e => e.PlanId == planId && e.ImageLevel > 0).ToList();
			ret.AddRange(tempImages);

			return ret;

		}

		public PlanImage GetPublicPlanTopImage(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			if (planId <= 0)
			{
				throw new YrsWebException("プランIDが不正です");
			}

			PublicPlanInfo planInfo = base.DbContext.PublicPlanInfo.SingleOrDefault(e => e.PlanId == planId);
			if (planInfo == null)
			{
				return null;
			}

			//トップ画像
			PlanImage topImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == planId && e.ImageLevel <= 0);
			if (topImage == null)
			{
				return null;
			}

			return topImage;
		}

		public PlanImage GetPublicPlanTempImage(int planId, byte imageNo)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			if (planId <= 0)
			{
				throw new YrsWebException("プランIDが不正です");
			}


			PublicPlanInfo planInfo = base.DbContext.PublicPlanInfo.SingleOrDefault(e => e.PlanId == planId);
			if (planInfo == null)
			{
				return null;
			}

			//その他画像
			PlanImage tempImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == planId && e.ImageNo == imageNo);
			if (tempImage == null)
			{
				return null;
			}

			return tempImage;
		}

		#endregion

		#region 申込関連
		/// <summary>
		/// 指定されたプランID（と申込日）のリストから、申込新規登録用のモデルを作成して返します
		/// </summary>
		/// <param name="appEditModelRequest"></param>
		/// <returns></returns>
		public AppEditModel GetNewAppEditModel(AppEditModelRequest appEditModelRequest)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			AppEditModel ret = new AppEditModel();

			//ヘッダ新規作成
			ApplicationHeader header = new ApplicationHeader();
			header.Num_Adult = 1;//大人１名は必須
			ret.ApplicationHeader = header;

			//プラン情報を設定（モデルと、申込プラン）
			foreach (AppEditModelRequestItem reqItem in appEditModelRequest.PlanIdList)
			{
				//プランモデル取得
				PublicPlanModel planModel = this.GetPlanModel(reqItem.PlanId);
				if (planModel == null) continue;

				ret.PlanModelList.Add(planModel);

				//申込プラン 新規作成
				ApplicationPlan appPlan = new ApplicationPlan();

				appPlan.ApplicationId = -1;//POST時に採番
				appPlan.PlanId = planModel.PlanInfo.PlanId;
				appPlan.AcceptDate = DateTime.Parse(reqItem.AcceptDate);
				appPlan.Fee_Adult = planModel.PlanInfo.Fee_Adult;
				appPlan.Fee_Child = planModel.PlanInfo.Fee_Child;
				appPlan.Fee_Infant = planModel.PlanInfo.Fee_Infant;
				//appPlan.InsertDate 
				//appPlan.UpdateDate
				appPlan.DeleteFlg = false;

				ret.ApplicationPlans.Add(appPlan);
			}

			//申込ユーザを１人だけ追加
			ApplicationUser user = new ApplicationUser();
			user.UserNo = 0;//代表者
			ret.ApplicationUsers.Add(user);

			return ret;

		}

		private string GetOrderNo(AppEditModel appEditModel)
		{

			string ret = Convert.ToString(appEditModel.ApplicationHeader.ApplicationId);

			ret = ret.PadLeft(8, '0');


			/*"0：クレジットカード払い
			1：銀行振込
			2：現地支払"
			*/
			switch (appEditModel.ApplicationHeader.PaymentMethods)
			{
				case 0:
					ret = "CC" + ret;
					break;
				case 1:
					ret = "BT" + ret;
					break;
				case 2:
					ret = "CA" + ret;
					break;

			}
			return ret;
		}

		public AppEditModel PutAppEditModel(AppEditModel appEditModel)
		{

			//存在チェック(過去削除されたものも含めてチェック）
			if (base.DbContext.ApplicationHeader.Any(e => e.ApplicationId == appEditModel.ApplicationHeader.ApplicationId))
				throw new YrsWebException("この申込は既に登録されています");

			//同行者のチェック
			if (appEditModel.IsAllPerson && appEditModel.ApplicationUsers.Count < appEditModel.ApplicationHeader.Num_Adult)
				throw new YrsWebException("同行者情報が不足しています");

			//処理日付
			DateTime now = DateTime.Now;

			//トランザクション管理
			var tran = this.DbContext.Database.BeginTransaction();
			try
			{
				//Header
				appEditModel.ApplicationHeader.InsertDate = now;
				appEditModel.ApplicationHeader.UpdateDate = now;
				appEditModel.ApplicationHeader.DeleteFlg = false;
				appEditModel.ApplicationHeader.OrderNo = "";//一旦ブランク
				appEditModel.ApplicationHeader = base.DbContext.ApplicationHeader.Add(appEditModel.ApplicationHeader);

				//申込IDを採番するため、一旦Saveする
				this.DbContext.SaveChanges();

				//申込ID採番後、注文番号を採番しなおし
				appEditModel.ApplicationHeader.OrderNo = this.GetOrderNo(appEditModel);

				//Plan
				foreach (ApplicationPlan plan in appEditModel.ApplicationPlans)
				{
					plan.ApplicationId = appEditModel.ApplicationHeader.ApplicationId;

					if (base.DbContext.ApplicationPlan.Any(e => (e.ApplicationId == plan.ApplicationId && e.PlanId == plan.PlanId)))
						throw new YrsWebException("この申込プランは既に登録されています");

					plan.InsertDate = now;
					plan.UpdateDate = now;
					plan.DeleteFlg = false;

					this.DbContext.ApplicationPlan.Add(plan);
				}

				//User
				//代表者
				ApplicationUser mainUser = appEditModel.ApplicationUsers.SingleOrDefault(e => e.UserNo == 0);
				if (mainUser == null)
					throw new YrsWebException("代表者情報が登録されていません");

				mainUser.ApplicationId = appEditModel.ApplicationHeader.ApplicationId;
				mainUser.InsertDate = now;
				mainUser.UpdateDate = now;
				mainUser.DeleteFlg = false;

				this.DbContext.ApplicationUser.Add(mainUser);


				//同行者
				List<ApplicationUser> subUsers = appEditModel.ApplicationUsers.Where(e => e.UserNo != 0).OrderBy(e => e.UserNo).ToList();

				//TODO:代表者データをどうするか、、、
				int userNo = 1;
				foreach (ApplicationUser subUser in subUsers)
				{
					subUser.ApplicationId = appEditModel.ApplicationHeader.ApplicationId;
					subUser.UserNo = userNo;

					if (base.DbContext.ApplicationUser.Any(e => (e.ApplicationId == subUser.ApplicationId && e.UserNo == subUser.UserNo)))
						throw new YrsWebException("この申込ユーザは既に登録されています");

					subUser.InsertDate = now;
					subUser.UpdateDate = now;
					subUser.DeleteFlg = false;

					this.DbContext.ApplicationUser.Add(subUser);
					userNo++;
				}

				//DB保存
				base.DbContext.SaveChanges();
				tran.Commit();
			}
			catch
			{
				tran.Rollback();
				throw;
			}


			//受付完了メールの送信
			if (appEditModel.ApplicationHeader.PaymentMethods != 0)
			{
				//決済方法 がクレジットカード決済 以外（銀行振込、現地支払）
				if (this.mailBiz == null) this.mailBiz = new SendAppCompMailBiz(base.Controller, base.DbContext, this._hostingEnvironment);

				//管理者向け
				this.mailBiz.SendAppCompMail_Manager(appEditModel);

				//事業者向け
				this.mailBiz.SendAppCompMail_Provider(appEditModel);

				//顧客向け
				this.mailBiz.SendAppCompMail_Customer(appEditModel);
			}

			return appEditModel;
		}

		/// <summary>
		/// 指定された申込IDから、既存の申込モデルを作成して返します
		/// </summary>
		/// <param name="applicationId"></param>
		/// <returns></returns>
		public AppEditModel GetExistAppEditModel(int applicationId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			AppEditModel ret = new AppEditModel();

			//ヘッダ
			ApplicationHeader header = base.DbContext.ApplicationHeader.SingleOrDefault(e => e.ApplicationId == applicationId);
			if (header == null) throw new YrsWebException("この申込は存在しません");
			ret.ApplicationHeader = header;

			//プラン
			List<ApplicationPlan> plans = base.DbContext.ApplicationPlan.Where(e => e.ApplicationId == applicationId).ToList();
			ret.ApplicationPlans = plans;

			//プランモデル
			foreach (ApplicationPlan plan in plans)
			{
				PublicPlanModel planModel = this.GetPlanModel(plan.PlanId);
				ret.PlanModelList.Add(planModel);
			}

			//ユーザ
			List<ApplicationUser> users = base.DbContext.ApplicationUser.Where(e => e.ApplicationId == applicationId).ToList();
			ret.ApplicationUsers = users;

			//決済情報
			List<ApplicationPayment> payments = base.DbContext.ApplicationPayment.Where(e => e.ApplicationId == applicationId).ToList();
			ret.ApplicationPayments = payments;


			return ret;
		}



		/// <summary>
		/// 指定された申込IDから、既存の申込（カード決済済）モデルを作成して返します
		/// </summary>
		/// <param name="applicationId"></param>
		/// <returns></returns>
		public AppEditModel GetPayCompAppEditModel(int applicationId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			AppEditModel ret = new AppEditModel();

			//ヘッダ
			ApplicationHeader header = base.DbContext.ApplicationHeader.SingleOrDefault(e => e.ApplicationId == applicationId);
			if (header == null) throw new YrsWebException("この申込は存在しません");
			ret.ApplicationHeader = header;

			//決済情報
			List<ApplicationPayment> payments = base.DbContext.ApplicationPayment.Where(e => e.ApplicationId == applicationId).ToList();

			//ApplicationPayment paiedEt = payments.Where(e => e.PaymentStatus == 9).OrderByDescending(e => e.PaymentSeq).FirstOrDefault();
			//if (paiedEt == null) throw new YrsWebException("この申込は決済完了していません");
			ApplicationPayment paiedEt = payments.OrderByDescending(e => e.PaymentSeq).FirstOrDefault();
			if (paiedEt == null) throw new YrsWebException("この申込の決済情報が見つかりません");

			////TODO:DEBUG
			//paiedEt.ErrCode = "ERR";
			//paiedEt.ErrInfo = "TestError";

			if (paiedEt.InsertDate.AddMinutes(1) <= DateTime.Now)
			{
				throw new YrsWebException("表示期限を過ぎています");
			}

			ret.ApplicationPayments = payments;


			//プラン
			List<ApplicationPlan> plans = base.DbContext.ApplicationPlan.Where(e => e.ApplicationId == applicationId).ToList();
			ret.ApplicationPlans = plans;

			//プランモデル
			foreach (ApplicationPlan plan in plans)
			{
				PublicPlanModel planModel = this.GetPlanModel(plan.PlanId);
				ret.PlanModelList.Add(planModel);
			}

			//ユーザ
			List<ApplicationUser> users = base.DbContext.ApplicationUser.Where(e => e.ApplicationId == applicationId).ToList();
			ret.ApplicationUsers = users;



			return ret;
		}

		#endregion

		#region 個人情報保護方針
		public string GetPrivacyPolicyFromProviderId(string providerId)
		{

			Provider_M provider = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == providerId);

			if (provider == null)
			{
				throw new YrsWebException("指定された事業者が存在しません");
			}

			return provider.PersonalinfoManagement;
		}

		public string GetPrivacyPolicyFromPlanId(int planId)
		{
			PlanInfo plan = base.DbContext.PlanInfo.SingleOrDefault(e => e.PlanId == planId);
			if (plan == null)
			{
				throw new YrsWebException("指定されたプランが存在しません");
			}

			Provider_M provider = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == plan.ProviderId);

			if (provider == null)
			{
				throw new YrsWebException("指定された事業者が存在しません");
			}

			return provider.PersonalinfoManagement;
		}
		#endregion

		#region 仕様変更の為 未使用
		/*
		public ApplicationInfo GetApplication(int applicationId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			ApplicationInfo applicationInfo = base.DbContext.ApplicationInfo.SingleOrDefault(e => e.ApplicationId == applicationId
			&& e.DeleteFlg == false
			);
			return applicationInfo;
		}

		public ApplicationInfo InsertApplication(ApplicationInfo applicationInfo)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			DateTime now = DateTime.Now;

			if (this.DbContext.ApplicationInfo.Any(e => e.ApplicationId == applicationInfo.ApplicationId))
			{
				throw new YrsWebException("この申込情報は既に存在します");
			}

			//エラーチェック
			ApplicationInfo.CheckApplicationInfo(applicationInfo);

			//時刻設定
			applicationInfo.InsertDate = now;
			applicationInfo.UpdateDate = now;

			//追加
			applicationInfo = this.DbContext.ApplicationInfo.Add(applicationInfo);

			//コミット
			this.DbContext.SaveChanges();

			return applicationInfo;

		}

		public ApplicationInfo UpdateApplication(ApplicationInfo applicationInfo)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			DateTime now = DateTime.Now;

			if (!this.DbContext.ApplicationInfo.Any(e => e.ApplicationId == applicationInfo.ApplicationId))
			{
				throw new YrsWebException("この申込情報は存在しません");
			}

			//エラーチェック
			ApplicationInfo.CheckApplicationInfo(applicationInfo);

			//時刻設定
			applicationInfo.UpdateDate = now;

			//DB保存
			applicationInfo = this.DbContext.ApplicationInfo.Attach(applicationInfo);//DbContextにアタッチ

			//コミット
			this.DbContext.SaveChanges();

			return applicationInfo;

		}


		public int DeleteApplication(int applicationId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			DateTime now = DateTime.Now;

			ApplicationInfo target = this.DbContext.ApplicationInfo.SingleOrDefault(e => e.ApplicationId == applicationId);
			if (target == null)
			{
				throw new YrsWebException("この申込情報は存在しません");
			}

			//論理削除
			target.DeleteFlg = true;
			target.UpdateDate = now;

			//コミット
			base.DbContext.SaveChanges();

			return applicationId;

		}
		*/

		#endregion

	}
}
