namespace PyroCloud.Modules.Identity.Authorization
{
    public static class IdentityPermissions
    {
        public const string GroupName = "Identity";

        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Manage = Default + ".Manage";
        }

        public static class Tenants
        {
            public const string Default = GroupName + ".Tenants";
            public const string Create = Default + ".Create";
        }
    }
}
