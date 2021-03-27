using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YrsWeb
{
	public static class Statics
	{

		public const string SYSTEM_NAME = YrsWeb.Lib.Common.Constants.SYSTEM_NAME;

		public const string CORS_PolicyName = "YRS_CORS_PolicyName";
		public const string ALLOW_ORIGIN_URL = "https://yoshino-reserve.com";

		public const string DEFAULT_MANAGER_ID = "YRS_MANAGER";

		public static List<string> UnAuthUrlList = new List<string>() {
			"/favicon.ico",
			"/TestMenu.html",
			"/resource",
			"/customer",
			"/api/ManagerAuth",
			"/api/ProviderAuth",
			"/pay",
			"/imgs",
			"/jobj",
		};


		public static string GetCommaNum(int num)
		{
			return num.ToString("#,0");
		}
	}
}
