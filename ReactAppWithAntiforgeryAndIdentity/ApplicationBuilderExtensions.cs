using Microsoft.AspNetCore.Builder;
using ReactAppWithAntiforgeryAndIdentity.Middleware;

namespace ReactAppWithAntiforgeryAndIdentity
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAntiforgeryTokens(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AntiforgeryTokenHandler>();
        }
    }
}
