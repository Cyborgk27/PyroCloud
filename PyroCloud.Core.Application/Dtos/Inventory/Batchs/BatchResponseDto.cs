namespace PyroCloud.Core.Application.Dtos.Inventory.Batchs
{
    public class BatchResponseDto
    {
        public Guid Id { get; set; }
        public string BatchNumber { get; set; } = default!;
        public int RemainingQuantity { get; set; }
        public decimal SalePrice { get; set; }

        // Agregamos estas dos para que la fórmula de abajo funcione
        public int DiscountThreshold { get; set; }
        public decimal DiscountAmount { get; set; }

        // Ahora sí, el contexto actual ya conoce estas variables
        public decimal CurrentDiscount => RemainingQuantity >= DiscountThreshold ? DiscountAmount : 0;
    }
}
