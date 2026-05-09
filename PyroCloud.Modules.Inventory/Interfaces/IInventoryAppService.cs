using PyroCloud.Core.Application.Dtos.Inventory.Batchs;

namespace PyroCloud.Modules.Inventory.Interfaces
{
    public interface IInventoryAppService
    {
        // Registro de una nueva compra (Crea un nuevo ProductBatch)
        Task<BatchResponseDto> AddStockAsync(Guid productId, CreateBatchDto input);

        // Obtiene los lotes activos (con stock) de un producto específico
        Task<List<BatchResponseDto>> GetActiveBatchesAsync(Guid productId);

        // Lógica clave: Calcula el precio real aplicando el descuento del lote 
        // según la cantidad que el cliente quiere comprar
        Task<decimal> GetUnitPriceAsync(Guid productId, int requestedQuantity);

        // Reduce el stock de los lotes (usualmente llamado desde el módulo de Ventas)
        // Implementa lógica FIFO (First In, First Out) por defecto
        Task DecreaseStockAsync(Guid productId, int quantity);
    }
}
