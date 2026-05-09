using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Modules.Inventory.Interfaces;
using PyroCloud.Modules.Inventory.Services;

namespace PyroCloud.Modules.Inventory.Extensions
{
    public static class InventoryModuleExtension
    {
        public static IServiceCollection AddInventoryModule(this IServiceCollection services)
        {
            services.AddScoped<IInventoryAppService, InventoryAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            return services;
        }
    }
}
