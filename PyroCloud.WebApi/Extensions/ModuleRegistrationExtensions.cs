using PyroCloud.Modules.Auth.Extensions;

namespace PyroCloud.WebApi.Extensions
{
    public static class ModuleRegistrationExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthModule();

            return services;
        }
    }
}
