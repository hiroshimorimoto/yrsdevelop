using System;
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

namespace YrsWeb.Controllers
{
	[Route("api")]
	public partial class ApiController : BaseController
	{

		public ApiController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings)
		: base(dbContext, fileClient, yrsAppSettings) { }

		[HttpGet("{bizName}/{resourceName}")]
		[Produces("application/json")]
		public ApiResult<string> Get(string bizName, string resourceName)
		{
			object bizObject = this.GetBizObject(bizName);
			return this.GetResult(bizObject, resourceName, (string)null);
		}


		//[HttpPost("{bizName}/{methodName}")]
		//[Produces("application/json")]
		//public ApiResult<string> Post(string bizName, string methodName, string body)
		//{
		//    Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
		//    Response.Headers["Access-Control-Allow-Credentials"] = "true";
		//    object bizObject = this.GetBizObject(bizName);
		//    return this.GetResult(bizObject, methodName, body);
		//}

		[HttpPost("{bizName}/{methodName}")]
		[Produces("application/json")]
		public ApiResult<string> Post(string bizName, string methodName, [FromBody] JObject jo)
		{
#if DEBUG
			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
#else
			Response.Headers["Access-Control-Allow-Origin"] = "https://yoshino-reserve.com/";
#endif
			Response.Headers["Access-Control-Allow-Credentials"] = "true";
			object bizObject = this.GetBizObject(bizName);
			return this.GetResult(bizObject, methodName, jo);
		}

		[HttpPost("{bizName}")]
		[Produces("application/json")]
		public ApiResult<string> PostWithBizMethod(string bizName, [FromBody] JObject jo)
		{
			string bizMethodJson = jo.ToString(Formatting.None);

			object bizObject = this.GetBizObject(bizName);

			BizMethodInfo bizMethodInfoObj = JsonConvert.DeserializeObject<BizMethodInfo>(bizMethodJson);
			return this.GetResult(bizObject, bizMethodInfoObj);
		}

		[HttpPost("form/{bizName}/{methodName}")]
		[Produces("application/json")]
		public ApiResult<string> PostWithFiles(string bizName, string methodName, string jsonData, List<IFormFile> files)
		{
#if DEBUG
			Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080/";
#else
			Response.Headers["Access-Control-Allow-Origin"] = "https://yoshino-reserve.com/";
#endif
			Response.Headers["Access-Control-Allow-Credentials"] = "true";
			object bizObject = this.GetBizObject(bizName);

			JObject jo = JObject.Parse(jsonData);
			return this.GetResult(bizObject, methodName, jo, files);
		}

		[HttpPost("form/{bizName}")]
		[Produces("application/json")]
		public ApiResult<string> PostWithBizMethodAndFiles(string bizName, string jsonData, List<IFormFile> files)
		{
			JObject jo = JObject.Parse(jsonData);
			string bizMethodJson = jo.ToString(Formatting.None);

			object bizObject = this.GetBizObject(bizName);

			BizMethodInfo bizMethodInfoObj = JsonConvert.DeserializeObject<BizMethodInfo>(bizMethodJson);
			return this.GetResult(bizObject, bizMethodInfoObj, files);
		}


		//[HttpPost("{bizName}")]
		//[Produces("application/json")]
		//public ApiResult<string> PostWithBizMethod(string bizName, string bizMethodInfo)
		//{
		//    object bizObject = this.GetBizObject(bizName);

		//    BizMethodInfo bizMethodInfoObj = JsonConvert.DeserializeObject<BizMethodInfo>(bizMethodInfo);
		//    return this.GetResult(bizObject, bizMethodInfoObj);
		//}


		private object GetBizObject(string bizName)
		{
			string bizClassName = String.Format("YrsWeb.Biz.{0}Biz", bizName);
			Type bizType = Type.GetType(bizClassName);
			object biz = Activator.CreateInstance(bizType, new object[] { this, this.DbContext });
			return biz;
		}

		private ApiResult<string> GetResult(object bizObject, string methodName, JObject jo, List<IFormFile> files = null)
		{
			ApiResult<string> result = new ApiResult<string>();
			try
			{
				object ret = null;

				Type bizType = bizObject.GetType();
				MethodInfo methodInfo = bizType.GetMethod(methodName);

				if (jo == null)
				{
					ret = methodInfo.Invoke(bizObject, new object[0]);
				}
				else
				{
					ParameterInfo pi = methodInfo.GetParameters()[0];
					string json = jo.ToString(Formatting.None, null);
					object param = JsonConvert.DeserializeObject(json, pi.ParameterType);
					if (files == null)
					{
						ret = methodInfo.Invoke(bizObject, new object[] { param });
					}
					else
					{
						ret = methodInfo.Invoke(bizObject, new object[] { param, files });
					}
				}

				result.ResultData = JsonConvert.SerializeObject(ret);
				return result;
			}
			catch (TargetInvocationException tiex)
			{
				return this.ErrorHandler(result, tiex);
			}
			catch (Exception ex)
			{
				throw new YrsWebException(ex.Message, ex);
			}
		}

		private ApiResult<string> GetResult(object bizObject, string methodName, string body)
		{
			ApiResult<string> result = new ApiResult<string>();
			try
			{
				object ret = null;

				Type bizType = bizObject.GetType();
				MethodInfo methodInfo = bizType.GetMethod(methodName);

				if (String.IsNullOrEmpty(body))
				{
					ret = methodInfo.Invoke(bizObject, new object[0]);
				}
				else
				{
					ParameterInfo pi = methodInfo.GetParameters()[0];
					object param = JsonConvert.DeserializeObject(body, pi.ParameterType);

					ret = methodInfo.Invoke(bizObject, new object[] { param });
				}

				result.ResultData = JsonConvert.SerializeObject(ret);
				return result;
			}
			catch (TargetInvocationException tiex)
			{
				return this.ErrorHandler(result, tiex);
			}
			catch (Exception ex)
			{
				throw new YrsWebException(ex.Message, ex);
			}
		}

		private ApiResult<string> GetResult(object bizObject, BizMethodInfo bizMethodInfo, List<IFormFile> files = null)
		{
			ApiResult<string> result = new ApiResult<string>();
			try
			{
				object ret = null;

				Type bizType = bizObject.GetType();

				List<Type> typeList = new List<Type>(bizMethodInfo.GetParameterTypes());
				List<object> valueList = new List<object>(bizMethodInfo.GetParameterValues());

				if (files != null)
				{
					typeList.Add(typeof(List<IFormFile>));
					valueList.Add(files);
				}

				Type[] parameterTypes = typeList.ToArray();
				object[] parameterValues = valueList.ToArray();

				MethodInfo methodInfo = bizType.GetMethod(bizMethodInfo.Name, parameterTypes);
				ret = methodInfo.Invoke(bizObject, parameterValues);

				result.ResultData = JsonConvert.SerializeObject(ret);
				return result;
			}
			catch (TargetInvocationException tiex)
			{
				return this.ErrorHandler(result, tiex);
			}
			catch (Exception ex)
			{
				throw new YrsWebException(ex.Message, ex);
			}
		}

	}
}
