using PyroCloud.Core.Domain.Common;
using PyroCloud.Core.Domain.Exceptions;

namespace PyroCloud.Core.Domain.Entities.Inventory
{
    public class ProductBatch : BaseEntity<Guid>, ITenantEntity
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = default!;

        // Información del Lote
        public string BatchNumber { get; set; } = default!;
        public DateTime ReceptionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Control de Stock
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }

        // Reglas de Precios (Dinero)
        public decimal PurchasePrice { get; set; } // Precio de compra (Costo)
        public decimal SalePrice { get; set; }     // Precio de venta sugerido para este lote

        // Reglas de Descuento (Negocio)
        public int DiscountThreshold { get; set; } // Ejemplo: 10 unidades
        public decimal DiscountAmount { get; set; } // Valor a descontar si llega al umbral

        public Guid? TenantId { get; set; } // Multitenencia activa

        // --- Reglas de Negocio (Domain Logic) ---

        public bool HasStock(int requestedQuantity) => RemainingQuantity >= requestedQuantity;

        public decimal CalculateUnitPrice(int quantityToBuy)
        {
            // Si la cantidad supera el umbral, aplicamos el descuento del lote
            if (quantityToBuy >= DiscountThreshold && DiscountThreshold > 0)
            {
                return SalePrice - DiscountAmount;
            }
            return SalePrice;
        }

        public void ReduceStock(int quantity)
        {
            if (!HasStock(quantity))
                throw new UserFriendlyException("Stock insuficiente en este lote específico.");

            RemainingQuantity -= quantity;
        }
    }
}
