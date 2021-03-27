using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System.Linq;
using System.Threading.Tasks;

using YrsWeb;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;

namespace YrsWeb.Middlewares
{
    public class YrsAuthMiddleware
    {
        private const string HEADER_NAME_NECESSARY_AUTH = "necessary-auth";

        private readonly RequestDelegate _next;

        public YrsAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            if (Statics.UnAuthUrlList.Any(e => path.StartsWith(e)))
            {
                //パスの先頭が、認証不要URLに一致する場合は、認証不要
                return this._next(context);
            }
            if (path.EndsWith(".html")　|| path.EndsWith(".js"))
            {
                //パスが、静的HTML、js に対するリクエストの場合 認証不要
                return this._next(context);
            }

            bool necessaryAuth = true;

            if (context.Request.Headers.ContainsKey(HEADER_NAME_NECESSARY_AUTH))
            {
                string buf = context.Request.Headers[HEADER_NAME_NECESSARY_AUTH][0];
                if(!bool.TryParse(buf,out necessaryAuth))
                {
                    necessaryAuth = true;
                }
            }

            //if (path.Contains("/api"))
            if(necessaryAuth)
            {
                ProviderLoginInfo providerLoginInfo = YrsWeb.SessionExtensions.Get<ProviderLoginInfo>(context.Session, ProviderLoginInfo.SESS_KEY_LOGIN_INFO);
                if (providerLoginInfo == null)
                {
                    ManagerLoginInfo loginInfo = YrsWeb.SessionExtensions.Get<ManagerLoginInfo>(context.Session, ManagerLoginInfo.SESS_KEY_LOGIN_INFO);
                    if (loginInfo == null)
                    {
                        throw new YrsUnAuthorizedException();
                    }
                }
            }

            return this._next(context);
        }
    }

    public static class YrsAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseYrsAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<YrsAuthMiddleware>();
        }
    }
}
