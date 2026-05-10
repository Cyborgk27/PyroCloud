using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Core.Application.Authorization;
using PyroCloud.Modules.Inventory.Authorization;
using PyroCloud.Modules.Inventory.Interfaces;
using PyroCloud.Modules.Inventory.Services;

namespace PyroCloud.Modules.Inventory.Extensions
{
    public static class InventoryModuleExtension
    {
        public static IServiceCollection AddInventoryModule(this IServiceCollection services)
        {
            PermissionRegistry.AddModulePermissions(InventoryPermissionDefinitionProvider.GetPermissions());

            services.AddScoped<IInventoryAppService, InventoryAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            return services;
        }
    }
}
