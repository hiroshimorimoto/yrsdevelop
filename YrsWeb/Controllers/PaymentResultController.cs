using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using System.Text;
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

namespace YrsWeb.Controllers
{

	[Route("pay")]
	public class PaymentResultController : BaseController
	{

		private IHostingEnvironment _hostingEnvironment;
		public PaymentResultController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings, IHostingEnvironment hostingEnvironment)
		: base(dbContext, fileClient, yrsAppSettings)
		{
			this._hostingEnvironment = hostingEnvironment;
		}


		//[HttpGet("result")]
		////[Produces("application/json")]
		//public IActionResult Get()
		//{
		//	PaymentResultEt payResEt = new PaymentResultEt(base.Request);

		//	Console.WriteLine(payResEt.TranID);

		//	return base.Ok();
		//}

		//private static string template = "";

		//static PaymentResultController()
		//{
		//	StringBuilder sb = new StringBuilder();

		//	sb.Append("<html>");
		//	sb.Append("<head>");
		//	sb.Append("< title > (自動的に転送されます) </ title >");
		//	sb.Append("</head>");
		//	sb.Append("<body>");
		//	sb.Append("</body>");
		//	sb.Append("</html>");
		//	template = sb.ToString();
		//}

		private string GetTemplate(String templateName)
		{
			String templatePath = Path.Combine(this._hostingEnvironment.ContentRootPath, "Templates", String.Format("{0}.cshtml", templateName));
			return System.IO.File.ReadAllText(templatePath);
		}


		//[HttpGet("mailtest/{type}/{applicationId}")]
		//[Produces("text/text")]
		//public IActionResult TemplateTest(string type, int applicationId)
		//{
		//	CustomerBiz biz = new CustomerBiz(this, base.DbContext,this._hostingEnvironment);
		//	AppEditModel model = biz.GetExistAppEditModel(applicationId);

		//	//string templateName = "AppCompMail_" + type;

		//	//if (!Engine.Razor.IsTemplateCached(templateName, typeof(AppEditModel)))
		//	//{
		//	//	string templateSource = this.GetTemplate(templateName);

		//	//	Engine.Razor.RunCompile(templateSource, templateName, typeof(AppEditModel), model);
		//	//}

		//	//string content = Engine.Razor.Run(templateName, typeof(AppEditModel), model);

		//	//return base.Content(content);

		//	string ret = "";
		//	SendAppCompMailBiz mailBiz = new SendAppCompMailBiz(this, base.DbContext, this._hostingEnvironment);
		//	switch(type){
		//		case "Manager":
		//			ret = mailBiz.SendAppCompMail_Manager(model);
		//			break;

		//		case "Provider":
		//			ret = mailBiz.SendAppCompMail_Provider(model);
		//			break;

		//		case "Customer":
		//			ret = mailBiz.SendAppCompMail_Customer(model);
		//			break;
		//	}
		//	return base.Content(ret); ;
		//}

		[HttpPost("result")]
		[Produces("application/json")]
		public IActionResult Post()
		{
			ApplicationPayment payResEt = new ApplicationPayment(base.Request);

			//申込決済情報を追加
			int seq = 0;

			if (base.DbContext.ApplicationPayment.Any(e => e.ApplicationId == payResEt.ApplicationId))
			{
				//存在する場合
				int maxSeq = base.DbContext.ApplicationPayment.Where(e => e.ApplicationId == payResEt.ApplicationId).Max(e => e.PaymentSeq);
				seq = maxSeq + 1;
			}

			payResEt.PaymentSeq = seq;

			if (String.IsNullOrEmpty(payResEt.ErrCode))
			{
				payResEt.PaymentStatus = 9;//決済済
			}
			else
			{
				//なにかしらの決済エラー
				payResEt.PaymentStatus = 0;//未決済
			}
			payResEt.InsertDate = DateTime.Now;

			base.DbContext.ApplicationPayment.Add(payResEt);
			base.DbContext.SaveChanges();



			CustomerBiz customerBiz = new CustomerBiz(this, base.DbContext, this._hostingEnvironment);
			AppEditModel appEditModel = customerBiz.GetExistAppEditModel(payResEt.ApplicationId);
			//受付完了メールの送信
			if (appEditModel.ApplicationHeader.PaymentMethods == 0)
			{
				//決済方法 がクレジットカード決済 
				SendAppCompMailBiz mailBiz = new SendAppCompMailBiz(this, base.DbContext, this._hostingEnvironment);

				//管理者向け
				mailBiz.SendAppCompMail_Manager(appEditModel);

				//事業者向け
				mailBiz.SendAppCompMail_Provider(appEditModel);

				//顧客向け
				mailBiz.SendAppCompMail_Customer(appEditModel);
			}

			return base.Redirect(String.Format("/application/index.html?appId={0}", payResEt.ApplicationId));
		}


		//[HttpPut("result")]
		////[Produces("application/json")]
		//public IActionResult Put()
		//{
		//	PaymentResultEt payResEt = new PaymentResultEt(base.Request);

		//	Console.WriteLine(payResEt.TranID);

		//	return base.Ok();
		//}
	}
}
