using Microsoft.Extensions.DependencyInjection;
using PyroCloud.Core.Application.Authorization;
using PyroCloud.Modules.Identity.Authorization;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Modules.Identity.Services;

namespace PyroCloud.Modules.Identity.Extensions
{
    public static class IdentityModuleExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            // 1. Registramos los permisos del módulo en el catálogo global automáticamente
            PermissionRegistry.AddModulePermissions(IdentityPermissionDefinitionProvider.GetPermissions());

            // 2. Registramos los servicios normales
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<ITenantAppService, TenantAppService>();
            services.AddScoped<ICompanyAppService, CompanyAppService>();

            return services;
        }
    }
}
