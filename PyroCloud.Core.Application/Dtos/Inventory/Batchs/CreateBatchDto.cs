using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroCloud.Core.Application.Dtos.Inventory.Batchs
{
    public class CreateBatchDto
    {
        public string BatchNumber { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; } // Costo de adquisición
        public decimal SalePrice { get; set; }     // Precio de venta para este lote

        // Reglas de descuento por cantidad
        public int DiscountThreshold { get; set; } // Ej: 10 unidades
        public decimal DiscountAmount { get; set; } // Ej: $2.00 de descuento

        public DateTime? ExpiryDate { get; set; }
    }
}
