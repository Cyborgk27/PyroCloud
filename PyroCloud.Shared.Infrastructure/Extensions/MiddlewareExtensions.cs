using Microsoft.AspNetCore.Builder;
using PyroCloud.Shared.Infrastructure.Middleware;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder app)
        {

            app.UseMiddleware<TenantMiddleware>();

            return app;
        }
    }
}   
