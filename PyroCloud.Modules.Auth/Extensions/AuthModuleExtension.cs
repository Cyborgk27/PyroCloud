using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Modules.Auth.Interfaces;
using PyroCloud.Modules.Auth.Services;

namespace PyroCloud.Modules.Auth.Extensions
{
    public static class AuthModuleExtensions
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services)
        {

            services.AddScoped<IAuthAppService, AuthAppService>();

            return services;
        }
    }
}
