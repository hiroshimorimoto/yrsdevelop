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
using YrsWeb.Biz;
namespace YrsWeb.Controllers
{

	[Route("imgs")]
	public class ImgsController : BaseController
	{
		public ImgsController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings)
		: base(dbContext, fileClient, yrsAppSettings) { }



		[HttpGet("plans/{planId}/top")]
		//[Produces("application/json")]
		public IActionResult GetTopImage(int planId)
		{
			PlanImage planImage = this.DbContext.PlanImage.FirstOrDefault(e => e.PlanId == planId && e.ImageLevel <= 0);
			if (planImage == null)
			{
				return base.NotFound(String.Format("画像が見つかりません Plan[{0}]", planId));
			}

			return this.ReturnFile(planImage);
		}


		[HttpGet("plans/{planId}/tmp/{imageNo}")]
		//[Produces("application/json")]
		public IActionResult GetTmpImage(int planId, byte imageNo)
		{
			PlanImage planImage = this.DbContext.PlanImage.SingleOrDefault(e => e.PlanId == planId && e.ImageNo == imageNo);
			if (planImage == null)
			{
				return base.NotFound(String.Format("画像が見つかりません Plan[{0}],Img[{1}]", planId, imageNo));
			}

			return this.ReturnFile(planImage);

		}

		private IActionResult ReturnFile(PlanImage planImage)
		{
			string path = String.Format("/Plan/{0}/images/{1}", planImage.PlanId, planImage.FileName);

			CloudFile cloudFile = FileStorageUtil.FindCloudFile(base.FileClient, base.YrsAppSettings.FileStorage_ShareName, path);

			if (cloudFile == null)
			{
				return base.NotFound(String.Format("画像が見つかりません Path[{0}]", path));
			}

			//return this.File(cloudFile.OpenRead(), "application/octet-stream", planImage.FileName);
			return this.File(cloudFile.OpenRead(), "image/jpeg", planImage.FileName);
		}

	}
}
