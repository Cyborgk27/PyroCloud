using PyroCloud.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroCloud.Core.Domain.Entities.Inventory
{
    public class Product : BaseEntity<Guid>, ITenantEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? SKU { get; set; } = default!; // Código único de inventario
        public string? Barcode { get; set; }
        public string? ImageUrl { get; set; }

        // Precio de referencia global (para el catálogo general)
        public decimal ReferencePrice { get; set; }

        // Multitenencia
        public Guid? TenantId { get; set; }

        // Relación con los lotes: Un producto tiene muchos lotes
        public virtual ICollection<ProductBatch> Batches { get; set; } = new List<ProductBatch>();

        // Propiedad calculada para saber el stock total sumando todos los lotes
        public int TotalStock => Batches?.Sum(b => b.RemainingQuantity) ?? 0;

    }
}
