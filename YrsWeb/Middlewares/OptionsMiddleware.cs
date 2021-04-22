using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System.Linq;
using System.Threading.Tasks;

using YrsWeb;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;

namespace YrsWeb.Middlewares
{
	public class OptionsMiddleware
	{

		private readonly RequestDelegate _next;

		public OptionsMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Method.Equals("Options", System.StringComparison.OrdinalIgnoreCase))
			{
				context.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080";
				//context.Response.Headers["Access-Control-Allow-Origin"] = "*";
				context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
				context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
				context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });

				//クッキーを引き継いでおく
				context.Response.Headers.Add("Set-Cookie", context.Request.Headers["Cookie"]);

				context.Response.StatusCode = StatusCodes.Status200OK;

				return Task.CompletedTask;
			}
			else
			{
				return this._next(context);
			}
		}
	}

	public static class OptionsMiddlewareExtensions
	{
		public static IApplicationBuilder UseOptionsMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<OptionsMiddleware>();
		}
	}
}
