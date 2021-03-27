using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;


namespace YrsWeb.Biz
{
	public class ProviderBiz : AbstractBiz
	{

		private FileBiz fileBiz;

		public ProviderBiz(ApiController controller, YRS_DBEntities dbContext) : base(controller, dbContext)
		{
			this.fileBiz = new FileBiz(controller, dbContext);
		}



		/// <summary>
		/// プラン単一選択
		/// </summary>
		/// <param name="planId"></param>
		/// <returns></returns>
		public PlanInfo GetPlan(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			PlanInfo ret = base.DbContext.PlanInfo.SingleOrDefault(e => e.PlanId == planId && !e.DeleteFlg);

			return ret;

		}



		/// <summary>
		/// プラン検索
		/// </summary>
		/// <param name="searchEt"></param>
		/// <returns></returns>
		public List<PlanInfo> GetPlanList(PlanListSearchEt searchEt)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			var q = base.DbContext.PlanInfo.AsQueryable();

			q = q.Where(e => e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId && !e.DeleteFlg);

			q = q.OrderBy(e => e.PlanId);

			List<PlanInfo> ret = q.ToList();

			return ret;
		}




		/// <summary>
		/// プランモデル取得
		/// </summary>
		/// <param name="planId"></param>
		/// <returns></returns>
		public PlanModel GetPlanModel(int planId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			PlanModel ret = new PlanModel();
			ret.EditModeType = EditModes.Read;

			ret.PlanEt = this.GetPlan(planId);

			//プラン受付日&受付日毎料金（別リストでモデリング）
			List<PlanDate> planDates = base.DbContext.PlanDate.Where(e => e.PlanId == planId).ToList();
			ret.AcceptDates = planDates.Select(e => e.AcceptDate.ToString("yyyy/MM/dd")).ToList();
			ret.DateFeeList = planDates.Where(e => e.Fee_Adult.HasValue || e.Fee_Child.HasValue || e.Fee_Infant.HasValue).ToList();

			//カテゴリリスト
			ret.SelectedCategoryIds = base.DbContext.PlanCategory.Where(e => e.PlanId == planId).Select(e => e.CategoryId).ToList();

			//エリアリスト
			ret.SelectedAreaIds = base.DbContext.PlanArea.Where(e => e.PlanId == planId).Select(e => e.AreaId).ToList();

			//トップ画像
			PlanImage topPlanImage = base.DbContext.PlanImage.Where(e => e.PlanId == planId && e.ImageLevel <= 0).FirstOrDefault();
			ret.TopImage = topPlanImage;

			//その他画像
			List<PlanImage> tempImageFiles = base.DbContext.PlanImage.Where(e => e.PlanId == planId && e.ImageLevel > 0).ToList();
			ret.TempImages = tempImageFiles;

			return ret;

		}

		//private static Func<Nullable<int>, bool> _hasFee = (Nullable<int> fee) =>
		//{
		//	if (fee.HasValue)
		//	{

		//	}
		//	else
		//	{
		//		return false;
		//	}
		//}

		/// <summary>
		/// プラン登録
		/// </summary>
		/// <param name="planModel"></param>
		/// <returns></returns>
		public PlanModel PostPlan(PlanModel planModel, List<IFormFile> files)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			DateTime now = DateTime.Now;

			if (planModel.EditModeType == EditModes.Add)
			{//追加
				planModel = this._insertPlan(planModel, files);
			}
			else if (planModel.EditModeType == EditModes.Edit)
			{//更新
				planModel = this._updatePlan(planModel, files);
			}
			//else if (planModel.EditModeType == EditModes.Delete)
			//{//削除
			//    planModel = this._deletePlan(planModel);
			//}

			return planModel;
		}


		private static IFormFile FindFormFile(string fileName, List<IFormFile> files)
		{
			if (files == null || files.Count <= 0)
			{
				return null;
			}

			IFormFile ret = files.FirstOrDefault(e => e.FileName == fileName);
			return ret;
		}

		private PlanModel _insertPlan(PlanModel planModel, List<IFormFile> files)
		{
			//存在チェック(過去削除されたものも含めてチェック）
			if (base.DbContext.PlanInfo.Any(e => e.PlanId == planModel.PlanEt.PlanId))
				throw new YrsWebException("このプランは既に登録されています");

			//トランザクション管理
			var tran = this.DbContext.Database.BeginTransaction();
			try
			{
				//DB保存
				planModel.PlanEt.ProviderId = ProviderLoginInfo.Instance.Provider.ProviderId;
				planModel.PlanEt.DeleteFlg = false;
				planModel.PlanEt = this.DbContext.PlanInfo.Add(planModel.PlanEt);
				//プランIDを採番するため、一旦Saveする
				this.DbContext.SaveChanges();

				//受付日、カテゴリ、エリアの保存
				this._postPlanSubModel(planModel);


				//現状の最大ImgNo
				byte imgNo = 1;

				//トップ画像の取得
				IFormFile topFormFile = FindFormFile(planModel.SelectedTopImageFileName, files);
				if (topFormFile != null)
				{
					files.Remove(topFormFile);

					PlanImage topImg = new PlanImage();
					topImg.PlanId = planModel.PlanEt.PlanId;
					topImg.ImageNo = imgNo;
					topImg.ImageLevel = 0;//TOP画像
					topImg.SortOrder = imgNo;//並び順（TOP画像は固定)
					topImg.FileName = topFormFile.FileName;
					topImg.Comment = null;//コメントなし

					//ファイルエンティティの追加
					this.DbContext.PlanImage.Add(topImg);

					//ファイルのアップロード
					this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, topImg.LocalImageFolder, topFormFile);

					imgNo++;
				}


				//その他のファイル
				if (planModel.TempImages != null)
				{
					foreach (PlanImage tempImage in planModel.TempImages)
					{
						if (tempImage.DeleteFlg) continue;


						if (tempImage.EditMode == "Add")
						{//新規追加
						 //画像の取得
							IFormFile tempFormFile = FindFormFile(tempImage.FileName, files);
							if (tempFormFile == null) continue;

							tempImage.PlanId = planModel.PlanEt.PlanId;
							tempImage.ImageNo = imgNo;

							this.DbContext.PlanImage.Add(tempImage);

							//ファイルのアップロード
							this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, tempImage.LocalImageFolder, tempFormFile);

							imgNo++;
						}
						else if (tempImage.EditMode == "Edit")
						{
							//更新？
							//画像の取得
							PlanImage oldImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == tempImage.PlanId && e.ImageNo == tempImage.ImageNo);

							IFormFile tempFormFile = FindFormFile(tempImage.FileName, files);
							if (tempFormFile != null)
							{
								if (oldImage != null)
								{
									//古いファイルを削除
									this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, oldImage.LocalImageFolder, oldImage.FileName);
								}
								else
								{
									//古いファイルを削除(Try)
									this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, String.Format("Plan/{0}/images", tempImage.PlanId), tempImage.FileName);
								}
								//ファイルのアップロード
								this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, tempImage.LocalImageFolder, tempFormFile);
							}

							oldImage.ImageLevel = tempImage.ImageLevel;
							oldImage.SortOrder = tempImage.SortOrder;
							oldImage.FileName = tempImage.FileName;
							oldImage.Comment = tempImage.Comment;

						}
					}
				}
				base.DbContext.SaveChanges();
				tran.Commit();
			}
			catch
			{
				tran.Rollback();
				throw;
			}
			return planModel;
		}

		private PlanModel _updatePlan(PlanModel planModel, List<IFormFile> files)
		{
			//存在チェック
			if (!base.DbContext.PlanInfo.Any(e => e.PlanId == planModel.PlanEt.PlanId && !e.DeleteFlg))
				throw new YrsWebException("プランが存在しません");
			//トランザクション管理
			var tran = this.DbContext.Database.BeginTransaction();
			try
			{
				//DB保存
				planModel.PlanEt = this.DbContext.PlanInfo.Attach(planModel.PlanEt);//DbContextにアタッチ

				//受付日、カテゴリ、エリアの保存
				this._postPlanSubModel(planModel);


				//現状の最大ImgNo
				byte imgNo = 1;
				List<byte> imgNoList = base.DbContext.PlanImage.Where(e => e.PlanId == planModel.PlanEt.PlanId).Select(e => e.ImageNo).ToList();
				if (imgNoList.Count > 0)
				{
					imgNo = imgNoList.Max();
					imgNo++;//インクリメント
				}

				//トップ画像の取得
				IFormFile topFormFile = FindFormFile(planModel.SelectedTopImageFileName, files);

				if (topFormFile != null)
				{
					files.Remove(topFormFile);
					PlanImage topImg = null;

					if (planModel.TopImage == null)
					{//Top画像エンティティの新規追加
						topImg = new PlanImage();
						topImg.PlanId = planModel.PlanEt.PlanId;
						topImg.ImageNo = imgNo;
						topImg.ImageLevel = 0;//TOP画像
						topImg.SortOrder = 0;//並び順（TOP画像は固定)
						topImg.FileName = topFormFile.FileName;
						topImg.Comment = null;//コメントなし

						//ファイルエンティティの追加
						this.DbContext.PlanImage.Add(topImg);

						imgNo++;
					}
					else
					{//Top画像エンティティの更新
					 //古いファイルを削除
						this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, planModel.TopImage.LocalImageFolder, planModel.TopImage.FileName);

						topImg = planModel.TopImage;

						//ファイルエンティティのアタッチ
						this.DbContext.PlanImage.Attach(topImg);

						topImg.ImageLevel = 0;//TOP画像
						topImg.SortOrder = 0;//並び順（TOP画像は固定)
						topImg.FileName = topFormFile.FileName;
					}

					//ファイルのアップロード
					this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, topImg.LocalImageFolder, topFormFile);
				}


				//その他のファイル
				foreach (PlanImage tempImage in planModel.TempImages)
				{
					if (tempImage.DeleteFlg)
					{//削除
						PlanImage oldImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == tempImage.PlanId && e.ImageNo == tempImage.ImageNo);
						if (oldImage != null)
						{
							//古いファイルを削除
							this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, String.Format("Plan/{0}/images", oldImage.PlanId), oldImage.FileName);

							this.DbContext.PlanImage.Remove(oldImage);
						}
						else
						{
							//古いファイルを削除(Try)
							this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, String.Format("Plan/{0}/images", tempImage.PlanId), tempImage.FileName);
						}

						//base.DbContext.PlanImage.Attach(tempImage);
						//base.DbContext.Entry<PlanImage>(tempImage).State = System.Data.Entity.EntityState.Deleted;
					}
					else
					{
						if (tempImage.EditMode == "Add")
						{//新規追加
						 //画像の取得
							IFormFile tempFormFile = FindFormFile(tempImage.FileName, files);
							if (tempFormFile == null) continue;

							tempImage.PlanId = planModel.PlanEt.PlanId;
							tempImage.ImageNo = imgNo;

							this.DbContext.PlanImage.Add(tempImage);

							//ファイルのアップロード
							this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, tempImage.LocalImageFolder, tempFormFile);

							imgNo++;
						}
						else if (tempImage.EditMode == "Edit")
						{
							//更新？
							//画像の取得
							PlanImage oldImage = base.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == tempImage.PlanId && e.ImageNo == tempImage.ImageNo);

							IFormFile tempFormFile = FindFormFile(tempImage.FileName, files);
							if (tempFormFile != null)
							{
								if (oldImage != null)
								{
									//古いファイルを削除
									this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, oldImage.LocalImageFolder, oldImage.FileName);
								}
								else
								{
									//古いファイルを削除(Try)
									this.fileBiz.DeleteFile(base.Controller.YrsAppSettings.FileStorage_ShareName, String.Format("Plan/{0}/images", tempImage.PlanId), tempImage.FileName);
								}
								//ファイルのアップロード
								this.fileBiz.SaveFile(base.Controller.YrsAppSettings.FileStorage_ShareName, tempImage.LocalImageFolder, tempFormFile);
							}


							oldImage.ImageLevel = tempImage.ImageLevel;
							oldImage.SortOrder = tempImage.SortOrder;
							oldImage.FileName = tempImage.FileName;
							oldImage.Comment = tempImage.Comment;

						}
					}
				}

				DbContext.Entry(planModel.PlanEt).State = System.Data.Entity.EntityState.Modified;
				base.DbContext.SaveChanges();
				tran.Commit();
			}
			catch
			{
				tran.Rollback();
				throw;
			}
			return planModel;
		}



		//private PlanModel _deletePlan(PlanModel planModel)
		//{
		//    this.DeletePlan(planModel.PlanEt.PlanId);
		//}


		private void _postPlanSubModel(PlanModel planModel)
		{
			int planId = planModel.PlanEt.PlanId;

			//プラン受付日のリプレイス
			base.DbContext.Database.ExecuteSqlCommand("DELETE FROM PlanDate WHERE PlanId = {0}", planId);
			planModel.AcceptDates.ForEach(acceptDateStr =>
			{
				if (DateTime.TryParse(acceptDateStr, out DateTime acceptDate))
				{
					PlanDate planDate = new PlanDate { PlanId = planId, AcceptDate = acceptDate };

					PlanDate dateFee = planModel.DateFeeList.SingleOrDefault(e =>
					{
						return (
							(e.Fee_Adult.HasValue || e.Fee_Child.HasValue || e.Fee_Infant.HasValue)
							&&
							e.AcceptDate.ToString("yyyy/MM/dd") == acceptDate.ToString("yyyy/MM/dd")
							);
					});

					if (dateFee == null)
					{
						planDate.Fee_Adult = new Nullable<int>();
						planDate.Fee_Child = new Nullable<int>();
						planDate.Fee_Infant = new Nullable<int>();
					}
					else
					{
						planDate.Fee_Adult = dateFee.Fee_Adult.HasValue ? dateFee.Fee_Adult.Value : new Nullable<int>();
						planDate.Fee_Child = dateFee.Fee_Child.HasValue ? dateFee.Fee_Child.Value : new Nullable<int>();
						planDate.Fee_Infant = dateFee.Fee_Infant.HasValue ? dateFee.Fee_Infant.Value : new Nullable<int>();
					}

					base.DbContext.PlanDate.Add(planDate);
				}
			});


			//プランカテゴリのリプレイス
			base.DbContext.Database.ExecuteSqlCommand("DELETE FROM PlanCategory	 WHERE PlanId = {0}", planId);
			planModel.SelectedCategoryIds.ForEach(categoryId =>
			{
				base.DbContext.PlanCategory.Add(new PlanCategory { PlanId = planId, CategoryId = categoryId });
			});


			//プランエリアの一括削除
			base.DbContext.Database.ExecuteSqlCommand("DELETE FROM PlanArea WHERE PlanId = {0}", planId);
			planModel.SelectedAreaIds.ForEach(areaId =>
			{
				base.DbContext.PlanArea.Add(new PlanArea { PlanId = planId, AreaId = areaId });
			});
		}

		/// <summary>
		/// プラン公開ステータス更新
		/// </summary>
		/// <param name="planId"></param>
		/// <returns></returns>
		public PlanInfo PostPlanPublicStatus(int planId, int publicStatus)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			DateTime now = DateTime.Now;

			PlanInfo ret = base.DbContext.PlanInfo.SingleOrDefault(e => e.PlanId == planId && !e.DeleteFlg);
			if (ret != null)
			{
				ret.PublicStatus = Convert.ToByte(publicStatus);

				if (ret.PublicStatus == 0)
				{
					//非公開
					ret.PublicEndDate = now;
				}
				else if (ret.PublicStatus == 1)
				{
					//公開
					ret.PublicStartDate = now;
					ret.PublicEndDate = null;
				}
			}
			base.DbContext.SaveChanges();

			return ret;
		}

		public PlanInfo DeletePlan(int planId)
		{
			//存在チェック
			PlanInfo planEt = base.DbContext.PlanInfo.SingleOrDefault(e => e.PlanId == planId && !e.DeleteFlg);
			if (planEt == null)
				throw new YrsWebException("プランが存在しません");

			//トランザクション管理
			var tran = this.DbContext.Database.BeginTransaction();
			try
			{
				//DB保存
				//planEt = this.DbContext.PlanInfo.Attach(planEt);//DbContextにアタッチ
				planEt.DeleteFlg = true;

				//DbContext.Entry(planModel.PlanEt).State = System.Data.Entity.EntityState.Modified;
				base.DbContext.SaveChanges();
				tran.Commit();
			}
			catch
			{
				tran.Rollback();
				throw;
			}
			return planEt;
		}




		public PlanAppInfo GetApplicationForProvider(int applicationId)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			PlanAppInfo ret = base.DbContext.PlanAppInfo.SingleOrDefault(e => e.ApplicationId == applicationId && e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId);
			return ret;
		}

		public List<PlanAppInfo> GetApplicationListForProvider()
		{

#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			List<PlanAppInfo> ret = base.DbContext.PlanAppInfo.Where(e => e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId).ToList();

			return ret;
		}

		public List<PlanAppInfo> GetApplicationListForPlan(int planId)
		{

#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

			List<PlanAppInfo> ret = base.DbContext.PlanAppInfo.Where(e => e.PlanId == planId && e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId).ToList();

			return ret;
		}


		public string GetPrivacyPolicy()
		{

			Provider_M provider = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId);

			if (provider == null)
			{
				throw new YrsWebException("指定された事業者が存在しません");
			}

			return provider.PersonalinfoManagement;
		}

		public string PostPrivacyPolicy(string privacyPolicy)
		{

			Provider_M provider = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId);

			if (provider == null)
			{
				throw new YrsWebException("指定された事業者が存在しません");
			}


			provider.PersonalinfoManagement = privacyPolicy;
			base.DbContext.SaveChanges();

			return provider.PersonalinfoManagement;
		}


		#region 仕様変更の為未使用
		/*

		public ApplicationInfo UpdateApplication(ApplicationInfo applicationInfo)
		{
#if DEBUG
			base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
			DateTime now = DateTime.Now;

			if (!this.DbContext.ApplicationInfo.Any(e => e.ApplicationId == applicationInfo.ApplicationId && e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId))
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

			ApplicationInfo target = this.DbContext.ApplicationInfo.SingleOrDefault(e => e.ApplicationId == applicationId && e.ProviderId == ProviderLoginInfo.Instance.Provider.ProviderId);
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
