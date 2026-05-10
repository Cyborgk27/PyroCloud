namespace PyroCloud.Modules.Inventory.Authorization
{
    public static class InventoryPermissions
    {
        public const string GroupName = "Inventory";

        public static class Products
        {
            public const string Default = GroupName + ".Products";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Stock
        {
            public const string Default = GroupName + ".Stock";
            public const string AddBatch = Default + ".AddBatch"; // Registrar compras
            public const string ViewPrices = Default + ".ViewPrices"; // Ver precios dinámicos
        }
    }
}
