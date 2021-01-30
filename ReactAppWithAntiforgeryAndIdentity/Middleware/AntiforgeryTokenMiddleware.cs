using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace ReactAppWithAntiforgeryAndIdentity.Middleware
{
    public class AntiforgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            AppendTokensAsCookies(context);

            string method = context.Request.Method;
            string path = context.Request.Path;

            if (path != "/connect/token" &&
                !HttpMethods.IsGet(method) &&
                !HttpMethods.IsHead(method) &&
                !HttpMethods.IsOptions(method) &&
                !HttpMethods.IsTrace(method))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }

            await _next(context);
        }

        private void AppendTokensAsCookies(HttpContext context)
        {
            if (context.Request.Path == "/")
            {
                var tokens = _antiforgery.GetTokens(context);
                if (tokens.CookieToken != null)
                {
                    // The CookieToken is not available when the client already has the cookie, hence the null check.
                    context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.CookieToken, new CookieOptions { HttpOnly = false });
                }
                context.Response.Cookies.Append("X-CSRF-FORM-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
            }
        }
    }
}
