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
using System.IO;
using YrsWeb.Biz;

namespace YrsWeb.Controllers
{
	[Route("files")]
	public class FilesController : BaseController
	{
		public FilesController(YRS_DBEntities dbContext, CloudFileClient fileClient, IYrsAppSettings yrsAppSettings)
		: base(dbContext, fileClient, yrsAppSettings) { }


		[HttpGet("{*path}")]
		public IActionResult Get(string path)
		{
			CloudFile cloudFile = FileStorageUtil.FindCloudFile(base.FileClient, base.YrsAppSettings.FileStorage_ShareName, path);

			if (cloudFile == null)
			{
				return base.NotFound(String.Format("ファイルが見つかりません Path[{0}]", path));
			}

			return this.File(cloudFile.OpenRead(), "application/octet-stream");
		}
	}
}
