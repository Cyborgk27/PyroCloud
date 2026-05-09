using PyroCloud.Modules.Auth.Extensions;
using PyroCloud.Modules.Identity.Extensions;
using PyroCloud.Modules.Inventory.Extensions;

namespace PyroCloud.WebApi.Extensions
{
    public static class ModuleRegistrationExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthModule();
            services.AddIdentityModule();
            services.AddInventoryModule();

            return services;
        }
    }
}
