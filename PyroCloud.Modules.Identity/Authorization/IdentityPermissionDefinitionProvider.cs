using PyroCloud.Core.Application.Common;

namespace PyroCloud.Modules.Identity.Authorization
{
    public static class IdentityPermissionDefinitionProvider
    {
        public static PermissionDto GetPermissions()
        {
            // Nodo Raíz del Módulo
            var identityGroup = new PermissionDto("Gestión de Identidad", IdentityPermissions.GroupName);

            // Subgrupo: Usuarios
            var users = new PermissionDto("Usuarios", IdentityPermissions.Users.Default);
            users.Children.Add(new PermissionDto("Crear", IdentityPermissions.Users.Create));
            users.Children.Add(new PermissionDto("Editar", IdentityPermissions.Users.Edit));
            users.Children.Add(new PermissionDto("Eliminar", IdentityPermissions.Users.Delete));

            // Subgrupo: Roles
            var roles = new PermissionDto("Roles", IdentityPermissions.Roles.Default);
            roles.Children.Add(new PermissionDto("Administrar Permisos", IdentityPermissions.Roles.Manage));

            // Subgrupo: Tenants
            var tenants = new PermissionDto("Multi-Tenancia (Empresas)", IdentityPermissions.Tenants.Default);
            tenants.Children.Add(new PermissionDto("Crear Empresa", IdentityPermissions.Tenants.Create));

            // Armamos el árbol
            identityGroup.Children.Add(users);
            identityGroup.Children.Add(roles);
            identityGroup.Children.Add(tenants);

            return identityGroup;
        }
    }
}