using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using PyroCloud.Shared.Infrastructure.Identity.Claims;
using PyroCloud.Shared.Infrastructure.Presistence.Context;
using PyroCloud.Shared.Infrastructure.Presistence.Interceptors;
using PyroCloud.Shared.Infrastructure.Services;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var infraSettings = new InfrastructureSettings();

            configuration.GetSection("Infrastructure").Bind(infraSettings);

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

            services.Configure<InfrastructureSettings>(configuration.GetSection("Infrastructure"));

            services.AddSingleton<IDateTime, DateTimeService>();

            services.AddTransient<IEmailService, SmtpEmailService>();

            services.AddScoped<IStorageService, LocalStorageService>();

            services.AddScoped<AuditInterceptor>();

            services.AddScoped<IUrlService, UrlService>();

            services.AddDbContext<PyroDbContext>((sp, options) =>
            {
                var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString(infraSettings.Database.SqlServer))
                       .AddInterceptors(auditInterceptor);
            });

            return services;
        }
    }
}
