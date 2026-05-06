using Microsoft.AspNetCore.Builder;
using PyroCloud.Shared.Infrastructure.Middlewares;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder app)
        {

            app.UseMiddleware<TenantMiddleware>();
            app.UseMiddleware<ApiResponseMiddleware>();

            return app;
        }
    }
}   
