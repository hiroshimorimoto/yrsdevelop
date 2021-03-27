using System;
using Microsoft.AspNetCore.Cors;
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
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace YrsWeb.Controllers
{
	[Route("jobj")]
	[EnableCors(YrsWeb.Statics.CORS_PolicyName)]
	public class CustomerController : BaseController
	{
		private CustomerBiz _customerBiz;
		private MasterDataBiz _masterDataBiz;

		public CustomerController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings, IHostingEnvironment hostingEnvironment)
		: base(dbContext, fileClient, yrsAppSettings)
		{
			this._customerBiz = new CustomerBiz(this, dbContext, hostingEnvironment);
			this._masterDataBiz = new MasterDataBiz(this, dbContext);
		}


		[HttpGet("categories")]
		[Produces("application/json")]
		public ApiResult<string> GetCategoryList()
		{
			return base.SimpleApiResult(this._masterDataBiz.GetCategoryList);
		}


		[HttpGet("plans")]
		[Produces("application/json")]
		public ApiResult<string> GetPlanList()
		{
			return base.SimpleApiResult(this._customerBiz.GetPlanList);
		}



		[HttpGet("plans/{planId}")]
		[Produces("application/json")]
		public ApiResult<string> GetPlanModel(int planId)
		{
			return base.SimpleApiResult(
				() => { return this._customerBiz.GetPlanModel(planId); },
				() => { throw new HttpException(404, String.Format("プランが見つかりません Plan[{0}]", planId)); }
			);
		}

		[HttpGet("plans/{planId}/imgs")]
		[Produces("application/json")]
		public ApiResult<string> GetPublicPlanImageListl(int planId)
		{
			return base.SimpleApiResult(() => { return this._customerBiz.GetPublicPlanImageList(planId); },
				() => { throw new HttpException(404, String.Format("プランイメージが見つかりません Plan[{0}]", planId)); }
			);
		}

		[HttpGet("plans/{planId}/pp")]
		[Produces("application/json")]
		public ApiResult<string> GetPrivacyPolicyFromPlanId(int planId)
		{
			return base.SimpleApiResult(
				() => { return this._customerBiz.GetPrivacyPolicyFromPlanId(planId); },
				() => { throw new HttpException(404, String.Format("個人情報管理規定が見つかりません", planId)); }
			);
		}

		[HttpGet("plans/{planId}/categories")]
		[Produces("application/json")]
		public ApiResult<string> GetPlanCategoryList(int planId)
		{
			return base.SimpleApiResult(
				() => { return this._customerBiz.GetPlanCategoryIdList(planId); },
				() => { throw new HttpException(404, String.Format("プランカテゴリが見つかりません Plan[{0}]", planId)); }
			);
		}


		[HttpGet("plans/{planId}/imgs/top")]
		[Produces("application/json")]
		public ApiResult<string> GetPublicPlanTopImage(int planId)
		{
			return base.SimpleApiResult(
				() => { return this._customerBiz.GetPublicPlanTopImage(planId); },
				() => { throw new HttpException(404, String.Format("画像が見つかりません Plan[{0}]", planId)); }
			);
		}

		[HttpGet("plans/{planId}/imgs/tmp/{imageNo}")]
		[Produces("application/json")]
		public ApiResult<string> GetPublicPlanTempImage(int planId, byte imageNo)
		{
			return base.SimpleApiResult(
				() => { return this._customerBiz.GetPublicPlanTempImage(planId, imageNo); },
				() => { throw new HttpException(404, String.Format("画像が見つかりません Plan[{0}],Img[{1}]", planId, imageNo)); }
			);
		}



		[HttpPost("plans")]
		[Produces("application/json")]
		public ApiResult<string> FindPlanList([FromBody] PlanListSearchEt searchEt)
		{
			//#if DEBUG
			//			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
			//#else
			//			Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
			//#endif
			//Response.Headers["Access-Control-Allow-Credentials"] = "true";

			return base.SimpleApiResult(() => { return this._customerBiz.FindPlanList(searchEt); });
		}


		[HttpPost("apps")]
		[Produces("application/json")]
		public ApiResult<string> GetNewAppEditModel([FromBody] AppEditModelRequest appEditModelRequest)
		{
			//#if DEBUG
			//			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
			//#else
			//			Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
			//#endif
			//Response.Headers["Access-Control-Allow-Credentials"] = "true";

			return base.SimpleApiResult(() => { return this._customerBiz.GetNewAppEditModel(appEditModelRequest); });
		}


		[HttpPost("apps/{applicationId}")]
		[Produces("application/json")]
		public ApiResult<string> GetExistAppEditModel(int applicationId)
		{
			//#if DEBUG
			//			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
			//#else
			//			Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
			//#endif
			//Response.Headers["Access-Control-Allow-Credentials"] = "true";

			return base.SimpleApiResult(() => { return this._customerBiz.GetExistAppEditModel(applicationId); });
		}

		[HttpGet("apps/{applicationId}")]
		[Produces("application/json")]
		public ApiResult<string> GetPayCompAppEditModel(int applicationId)
		{
			//#if DEBUG
			//			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
			//#else
			//			Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
			//#endif
			//Response.Headers["Access-Control-Allow-Credentials"] = "true";

			return base.SimpleApiResult(() => { return this._customerBiz.GetPayCompAppEditModel(applicationId); });
		}


		[HttpPut("apps")]
		[Produces("application/json")]
		public ApiResult<string> PutAppEditModel([FromBody] AppEditModel appEditModel)
		{
			//#if DEBUG
			//			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
			//#else
			//			Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
			//#endif
			//Response.Headers["Access-Control-Allow-Credentials"] = "true";

			return base.SimpleApiResult(() => { return this._customerBiz.PutAppEditModel(appEditModel); });
		}



		//[HttpPost("apps")]
		//[Produces("application/json")]
		//public ApiResult<string> InsertApplication([FromBody] ApplicationInfo applicationInfo)
		//{
		//	Response.Headers["Access-Control-Allow-Origin"] = Statics.ALLOW_ORIGIN_URL;
		//	Response.Headers["Access-Control-Allow-Credentials"] = "true";

		//	return base.SimpleApiResult(() => { return this._customerBiz.InsertApplication(applicationInfo); });
		//}

		//[HttpPut("apps")]
		//[Produces("application/json")]
		//public ApiResult<string> UpdateApplication([FromBody] ApplicationInfo applicationInfo)
		//{
		//	return base.SimpleApiResult(() => { return this._customerBiz.UpdateApplication(applicationInfo); });
		//}

		//[HttpDelete("apps/{applicationId}")]
		//[Produces("application/json")]
		//public ApiResult<string> DeleteApplication(int applicationId)
		//{
		//	return base.SimpleApiResult(() => { return this._customerBiz.DeleteApplication(applicationId); });
		//}
	}
}

