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
using System.Web;

namespace YrsWeb.Controllers
{
	public abstract class BaseController : Controller
	{
		private CloudFileClient _fileClient;
		private YRS_DBEntities _dbContext;
		private IYrsAppSettings _yrsAppSettings;

		public BaseController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings)
		{
			this._dbContext = dbContext;
			this._fileClient = fileClient;
			this._yrsAppSettings = yrsAppSettings;
		}
		public CloudFileClient FileClient => this._fileClient;

		public IYrsAppSettings YrsAppSettings => this._yrsAppSettings;

		public YRS_DBEntities DbContext => this._dbContext;

		protected ApiResult<string> SimpleApiResult(Func<object> func, Action actIfDataNull = null)
		{

			ApiResult<string> result = new ApiResult<string>();
			try
			{
				object ret = func();
				if (ret == null && actIfDataNull != null)
				{
					actIfDataNull();
				}

				result.ResultData = JsonConvert.SerializeObject(ret);
				return result;
			}
			catch (HttpException hex)
			{
				Response.StatusCode = hex.GetHttpCode();
				result.SetError(hex);
				return result;
			}
			catch (YrsWebException yrsWebEx)
			{
				Response.StatusCode = StatusCodes.Status500InternalServerError;
				result.SetError(yrsWebEx);
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

		protected ApiResult<string> ErrorHandler(ApiResult<string> result, TargetInvocationException tiex)
		{
			if (tiex.InnerException.GetType() == typeof(YrsUnAuthorizedException))
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				result.SetError(tiex.InnerException);
				return result;
			}
			else if (tiex.InnerException.GetType() == typeof(YrsWebException))
			{
				Response.StatusCode = StatusCodes.Status500InternalServerError;
				result.SetError(tiex.InnerException);
				return result;
			}
			else if (tiex.InnerException.GetType() == typeof(DbEntityValidationException))
			{
				DbEntityValidationException ex = (DbEntityValidationException)tiex.InnerException;

				Response.StatusCode = StatusCodes.Status500InternalServerError;

				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				foreach (var errors in ex.EntityValidationErrors)
				{
					foreach (var error in errors.ValidationErrors)
					{
						// VisualStudioの出力に表示
						//System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
						sb.Append(error.ErrorMessage);
						sb.Append("\r\n");
					}
				}
				result.SetError(new YrsWebException(sb.ToString(), ex));
				return result;
			}
			else if (tiex.InnerException.GetType() == typeof(SqlException))
			{
				SqlException ex = (SqlException)tiex.InnerException;

				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				foreach (SqlError error in ex.Errors)
				{
					// VisualStudioの出力に表示
					//System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
					sb.Append(error.Message);
					sb.Append("\r\n");
				}
				result.SetError(new YrsWebException(sb.ToString(), ex));
				return result;
			}
			else if (tiex.InnerException.GetType() == typeof(DbUpdateException))
			{
				DbUpdateException dbuEx = (DbUpdateException)tiex.InnerException;

				Exception bufEx = dbuEx;
				SqlException ex = null;
				while (true)
				{
					if (bufEx.InnerException == null) break;

					ex = bufEx.InnerException as SqlException;
					if (ex != null) break;

					bufEx = bufEx.InnerException;
				}

				if (ex == null)
				{
					result.SetError(new YrsWebException(dbuEx.Message, dbuEx));
				}
				else
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					foreach (SqlError error in ex.Errors)
					{
						// VisualStudioの出力に表示
						//System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
						sb.Append(error.Message);
						sb.Append("\r\n");
					}
					result.SetError(new YrsWebException(sb.ToString(), ex));
				}
				return result;

			}
			else
			{
				Response.StatusCode = StatusCodes.Status500InternalServerError;
				result.SetError(tiex.InnerException);
				return result;

				//throw new YrsWebException(tiex.InnerException.Message, tiex.InnerException);
				//throw tiex.InnerException;
			}
		}
	}
}
