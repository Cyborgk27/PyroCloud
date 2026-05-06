using Microsoft.AspNetCore.Http;

namespace PyroCloud.Shared.Infrastructure.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantId))
            {
                // 2. Guardarlo en un Scope para que el DbContext lo use
                // (Necesitarás un servicio tipo ITenantProvider para esto)
            }

            await _next(context);
        }
    }
}
