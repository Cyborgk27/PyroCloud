using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using PyroCloud.Shared.Infrastructure.Common.Settings;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class StorageMiddlewareExtensions
    {
        public static IApplicationBuilder UseLocalStorage(this IApplicationBuilder app)
        {
            var settings = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<IOptions<InfrastructureSettings>>().Value.Storage;

            var path = Path.Combine(Directory.GetCurrentDirectory(), settings.LocalPath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/cdn" // Los archivos se verán como: http://localhost:5000/cdn/archivo.jpg
            });

            return app;
        }
    }
}
