using PyroCloud.Core.Application.Dtos.Inventory.Products;

namespace PyroCloud.Modules.Inventory.Interfaces
{
    public interface IProductAppService
    {
        // Obtiene el producto con su stock total sumado de todos los lotes
        Task<ProductResponseDto> GetByIdAsync(Guid id);

        // Lista de productos con filtros básicos
        Task<List<ProductResponseDto>> GetListAsync();

        // Crea el producto y su lote inicial (Compra inicial)
        Task<ProductResponseDto> CreateAsync(CreateProductDto input);

        // Actualiza datos maestros (Nombre, descripción, etc.)
        Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto input);
    }
}
