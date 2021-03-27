using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using YrsWeb.Lib.Models;
using YrsWeb.Middlewares;

namespace YrsWeb
{
	public class Startup
	{
#if DEBUG
        private readonly string CorsPolicyName = "CorsPolicyName";
#endif

		internal static string DB_PREFIX;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{


#if DEBUG
			services.AddCors(options =>
			{
				options.AddPolicy(Statics.CORS_PolicyName,
					 builder => builder
						.AllowAnyHeader()
						.AllowCredentials()
						//.WithMethods("GET", "POST", "PUT")
						.AllowAnyMethod()
						.WithOrigins("http://localhost:8080")
					);
			});
#else
			services.AddCors(options =>
			{
				options.AddPolicy(Statics.CORS_PolicyName,
					 builder => builder
						.AllowAnyHeader()
						.AllowCredentials()
						//.WithMethods("GET", "POST", "PUT")
						.AllowAnyMethod()
						.WithOrigins(Statics.ALLOW_ORIGIN_URL)
					);
			});
#endif

			//インメモリ セッション を有効化
			int sessionTimeoutMin = (Int32)Configuration.GetSection("Session").GetValue(typeof(Int32), "TimeoutSec", 20);//タイムアウト値
			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeoutMin);
				options.Cookie.HttpOnly = true;
			});

			//DBコンテキストをDIコンテナに登録

			//デフォルトDB Prefixを取得
			DB_PREFIX = (string)Configuration.GetSection("Env").GetValue(typeof(string), "Prefix", "DEV");
			services.AddScoped<YRS_DBEntities>((IServiceProvider sp) =>
			{
				return new YRS_DBEntities(DB_PREFIX);
			});

			//Azure StorageをDIコンテナに登録
			services.AddScoped(_ => CloudStorageAccount.Parse(Configuration.GetConnectionString("YRSFiles")));

			// DI で CloudStorageAccount を取得して CloudFileClient を作成して返す
			services.AddScoped(provider => provider.GetService<CloudStorageAccount>().CreateCloudFileClient());

			//services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddMvc();


			//appsettings.jsonの読み込み
			IYrsAppSettings appSettings = this.ReadAppSettings();
			// DIにAppSettingsを設定
			services.AddSingleton<IYrsAppSettings>(_ =>
			{
				return this.ReadAppSettings();
			});

		}

		private IYrsAppSettings ReadAppSettings()
		{
			//appsettings.jsonの読み込み
			YrsAppSettingsImpl appSetting = new YrsAppSettingsImpl();

			appSetting.PassResetExpirationMin = (int)Configuration.GetSection("AppSetting").GetValue(typeof(int), "PassResetExpirationMin", "10");

			appSetting.SendMail_FromAddress = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_FromAddress", "");
			appSetting.SendMail_ManagerPassReset = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_ManagerPassReset", "");
			appSetting.SendMail_ProviderPassReset = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_ProviderPassReset", "");
			appSetting.SendMail_ProviderRegist = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_ProviderRegist", "");
			appSetting.SendMail_ProviderInitPass = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_ProviderInitPass", "");
			appSetting.SendMail_AppComp = (string)Configuration.GetSection("SendMail").GetValue(typeof(string), "SendMail_AppComp", "");

			appSetting.FileStorage_ShareName = (string)Configuration.GetSection("FileStorage").GetValue(typeof(string), "FileStorage_ShareName", "");

			return appSetting;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
#if DEBUG
            app.UseCors(CorsPolicyName);
#endif
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//インメモリ セッション を有効化
			app.UseSession();

			//YRS 例外ハンドラを設定
			app.UseYrsExceptionHandler();

			//YRS 認証ハンドラを設定
			app.UseYrsAuth();

			app.UseMvc();

			//CORS ポリシーを有効化
			app.UseCors(Statics.CORS_PolicyName);

			//静的ファイルの公開
			app.UseStaticFiles();
			app.UseDefaultFiles();

			//app.UseMvc(routes =>
			//{
			//    routes.MapRoute(
			//        name: "login",
			//        template: "{controller=login}/{action=index}");
			//});

			//if (env.IsDevelopment())
			//{
			//    app.UseDeveloperExceptionPage();
			//}
			//else
			//{
			//    app.UseExceptionHandler("/Home/Error");
			//    app.UseHsts();
			//}

			//app.UseHttpsRedirection();
			//app.UseStaticFiles();
			//app.UseCookiePolicy();

			//app.UseMvc(routes =>
			//{
			//    routes.MapRoute(
			//        name: "default",
			//        template: "{controller=Home}/{action=Index}/{id?}");
			//});


		}
	}
}
