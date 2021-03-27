using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

using YrsWeb.Lib.Common;

namespace YrsWeb.Middlewares
{
    public class YrsExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public YrsExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            HttpContext _context = context;
            try
            {
                string path = context.Request.Path;
                //Console.WriteLine(path);
                return this._next(context);

            }
            catch (YrsUnAuthorizedException)
            {
                throw;
            }
            catch (YrsWebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    
    public static class YrsExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseYrsExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<YrsExceptionMiddleware >();
        }
    }
    
}
