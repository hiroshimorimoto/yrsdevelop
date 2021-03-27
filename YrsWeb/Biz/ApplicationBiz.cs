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

namespace YrsWeb.Biz
{
	public class ApplicationBiz : AbstractBiz
	{
		public ApplicationBiz(BaseController controller, YRS_DBEntities dbContext) : base(controller, dbContext)
		{
		}
	}
}
