using PyroCloud.Core.Application.Common;

namespace PyroCloud.Modules.Inventory.Authorization;

public static class InventoryPermissionDefinitionProvider
{
    public static PermissionDto GetPermissions()
    {
        var inventoryGroup = new PermissionDto("Gestión de Inventario", InventoryPermissions.GroupName);

        // Subgrupo: Productos
        var products = new PermissionDto("Productos", InventoryPermissions.Products.Default);
        products.Children.Add(new PermissionDto("Crear", InventoryPermissions.Products.Create));
        products.Children.Add(new PermissionDto("Editar", InventoryPermissions.Products.Edit));
        products.Children.Add(new PermissionDto("Eliminar", InventoryPermissions.Products.Delete));

        // Subgrupo: Stock y Lotes
        var stock = new PermissionDto("Control de Stock (Lotes)", InventoryPermissions.Stock.Default);
        stock.Children.Add(new PermissionDto("Registrar Compras (Lotes)", InventoryPermissions.Stock.AddBatch));
        stock.Children.Add(new PermissionDto("Consultar Precios Dinámicos", InventoryPermissions.Stock.ViewPrices));

        inventoryGroup.Children.Add(products);
        inventoryGroup.Children.Add(stock);

        return inventoryGroup;
    }
}