using Microsoft.AspNetCore.Http;

namespace PyroCloud.Core.Application.Dtos.Inventory.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        // El SKU a veces se bloquea tras la creación, 
        // pero te lo dejo por si en PyroCloud permiten corregirlo.
        public string SKU { get; set; } = default!;

        public string? Barcode { get; set; }
        public IFormFile? ImageUrl { get; set; }

        // El precio de referencia global puede fluctuar 
        // independientemente de lo que costaron los lotes antiguos.
        public decimal ReferencePrice { get; set; }
    }
}
