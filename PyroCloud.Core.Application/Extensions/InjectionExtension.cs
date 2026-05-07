using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Application.Services;

namespace PyroCloud.Core.Application.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileAppService, FileAppService>();  
            return services;
        }
    }
}
