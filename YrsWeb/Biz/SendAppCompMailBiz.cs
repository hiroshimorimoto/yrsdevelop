using System;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;


using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.Azure; // Namespace for Azure Configuration Manager
using Microsoft.Azure.Storage; // Namespace for Storage Client Library
using Microsoft.Azure.Storage.File; // Namespace for Azure Files
using System.Data.Entity.Infrastructure;
using YrsWeb.Lib.Models;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Common;
using Newtonsoft.Json.Linq;
using YrsWeb.Biz;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using RazorEngine;
using RazorEngine.Templating;
using YrsWeb.Controllers;

namespace YrsWeb.Biz
{
	public class SendAppCompMailBiz : AbstractBiz
	{
		private IHostingEnvironment _hostingEnvironment;
		//private IYrsAppSettings _yrsAppSettings;


		private const string TEMPLATE_NAME_MANAGER = "AppCompMail_Manager";
		private const string TEMPLATE_NAME_PROVIDER = "AppCompMail_Provider";
		private const string TEMPLATE_NAME_CUSTOMER = "AppCompMail_Customer";


		public SendAppCompMailBiz(BaseController controller, YRS_DBEntities dbContext, IHostingEnvironment hostingEnvironment) : base(controller, dbContext)
		{
			this._hostingEnvironment = hostingEnvironment;
		}

		//public SendMailBiz(IHostingEnvironment hostingEnvironment, IYrsAppSettings yrsAppSettings)
		//{
		//	this._hostingEnvironment = hostingEnvironment;
		//	this._yrsAppSettings = yrsAppSettings;
		//}


		private void CompileTemplate(String templateName, AppEditModel appEditModel)
		{
			if (!Engine.Razor.IsTemplateCached(templateName, typeof(AppEditModel)))
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				//String[] names = asm.GetManifestResourceNames();
				//Console.WriteLine(names);

				String templatePath = String.Format("YrsWeb.Templates.{0}.cshtml", templateName);
				Stream stream = asm.GetManifestResourceStream(templatePath);
				if (stream == null) throw new YrsWebException(String.Format("テンプレ―ト[{0}]が読み込めません", templateName));
				StreamReader sr = new StreamReader(stream);
				string templateSource = sr.ReadToEnd();

				//String templatePath = Path.Combine(this._hostingEnvironment.ContentRootPath, "Templates", String.Format("{0}.cshtml", templateName));
				//templateSource = System.IO.File.ReadAllText(templatePath);
				Engine.Razor.RunCompile(templateSource, templateName, typeof(AppEditModel), appEditModel);
			}
		}



		public string SendAppCompMail_Manager(AppEditModel appEditModel)
		{
			//マネージャエンティティ
			Manager_M manager = base.DbContext.Manager_M.FirstOrDefault(e => e.ManagerId == Statics.DEFAULT_MANAGER_ID);
			if (manager == null)
			{
				throw new YrsWebException("運営者が存在しません");
			}

			//テンプレートを準備
			this.CompileTemplate(TEMPLATE_NAME_MANAGER, appEditModel);

			//メール本文を取得
			string mailBody = Engine.Razor.Run(TEMPLATE_NAME_MANAGER, typeof(AppEditModel), appEditModel);

			//メール送信 ロジックアプリ 呼び出しリクエスト
			JsonSerializer js = JsonSerializer.CreateDefault();
			var sendMailParam = new
			{
				from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
				to_address = manager.MailAddress,
				subject = "【よしのリザーブ】掲載中ツアーへの申込がありました",
				mail_body = mailBody
			};


			//メール送信 同期呼び出し
			return this._sendAppCompMail(sendMailParam);
		}



		public string SendAppCompMail_Customer(AppEditModel appEditModel)
		{
			ApplicationUser mainUser = appEditModel.ApplicationUsers.FirstOrDefault(e => e.UserNo == 0);
			if (mainUser == null)
			{
				throw new YrsWebException("代表者が存在しません");
			}

			//テンプレートを準備
			this.CompileTemplate(TEMPLATE_NAME_CUSTOMER, appEditModel);

			//メール本文を取得
			string mailBody = Engine.Razor.Run(TEMPLATE_NAME_CUSTOMER, typeof(AppEditModel), appEditModel);

			//メール送信 ロジックアプリ 呼び出しリクエスト
			var sendMailParam = new
			{
				from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
				to_address = mainUser.UserMailAddress,
				subject = "【よしのリザーブ】ツアーへのお申込を受付ました",
				mail_body = mailBody
			};

			//メール送信 同期呼び出し
			return this._sendAppCompMail(sendMailParam);

		}



		public string SendAppCompMail_Provider(AppEditModel appEditModel)
		{
			//テンプレートを準備
			this.CompileTemplate(TEMPLATE_NAME_PROVIDER, appEditModel);


			//プランリストをバックアップ
			List<PublicPlanModel> planModelList = new List<PublicPlanModel>(appEditModel.PlanModelList);
			List<ApplicationPlan> appPlans = new List<ApplicationPlan>(appEditModel.ApplicationPlans);

			//事業者毎にグルーピング
			var groupQuery = planModelList.GroupBy(e => e.PlanInfo.ProviderId);
			foreach (var group in groupQuery)
			{
				string providerId = group.Key;

				//対象事業者のプランのみに絞り込み
				//	プランモデル
				appEditModel.PlanModelList.Clear();
				appEditModel.PlanModelList.AddRange(group.ToList());
				//	申込プラン
				appEditModel.ApplicationPlans.Clear();
				appEditModel.ApplicationPlans.AddRange(
				appPlans.Where(appPlan => appEditModel.PlanModelList.Any(planModel => planModel.PlanInfo.PlanId == appPlan.PlanId))
				.ToList()
				);

				//代表プラン
				PublicPlanInfo mainPlan = appEditModel.PlanModelList[0].PlanInfo;

				//メール本文を取得
				string mailBody = Engine.Razor.Run(TEMPLATE_NAME_PROVIDER, typeof(AppEditModel), appEditModel);

				//メール送信 ロジックアプリ 呼び出しリクエスト
				var sendMailParam = new
				{
					from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
					to_address = mainPlan.TantoMailAddress,
					subject = "【よしのリザーブ】掲載中ツアーへのお申込がありました",
					mail_body = mailBody
				};

				//メール送信 同期呼び出し
				this._sendAppCompMail(sendMailParam);
			}

			//	プランモデル
			appEditModel.PlanModelList.Clear();
			appEditModel.PlanModelList.AddRange(planModelList);

			//	申込プラン
			appEditModel.ApplicationPlans.Clear();
			appEditModel.ApplicationPlans.AddRange(appPlans);

			return "200";
		}


		private string _sendAppCompMail(object sendMailParam)
		{
			JsonSerializer js = JsonSerializer.CreateDefault();
			StringBuilder sb = new StringBuilder();
			js.Serialize(new JsonTextWriter(new StringWriter(sb)), sendMailParam);

			string stringContent = sb.ToString();

			stringContent = stringContent.Replace(@"\r\n", "<br/>");

			//同期呼び出し
			var client = new HttpClient();
			HttpResponseMessage result = client.PostAsync(
				base.Controller.YrsAppSettings.SendMail_AppComp,
				 new StringContent(stringContent, System.Text.Encoding.UTF8, "application/json")
			).Result;


			var statusCode = result.StatusCode.ToString();
			return statusCode;

			//return "200";
		}

	}
}
