using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using PyroCloud.Shared.Infrastructure.Services;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InfrastructureSettings>(configuration.GetSection("Infrastructure"));

            services.AddSingleton<IDateTime, DateTimeService>();

            services.AddTransient<IEmailService, SmtpEmailService>();

            services.AddScoped<IStorageService, LocalStorageService>();

            return services;
        }
    }
}
