using PyroCloud.Core.Application.Dtos.Inventory.Batchs;

namespace PyroCloud.Core.Application.Dtos.Inventory.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }
        public decimal ReferencePrice { get; set; }

        // Al crear el producto, registramos su primer lote de una vez
        public CreateBatchDto InitialBatch { get; set; } = default!;
    }
}
