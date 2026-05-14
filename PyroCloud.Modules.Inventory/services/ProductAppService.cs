using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Dtos.Inventory.Batchs;
using PyroCloud.Core.Application.Dtos.Inventory.Products;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Entities.Inventory;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Modules.Inventory.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Inventory.Services;

public class ProductAppService : IProductAppService
{
    private readonly PyroDbContext _context;
    private readonly IFileAppService _fileAppService;
    private readonly IInventoryAppService _inventoryService;

    public ProductAppService(PyroDbContext context, IInventoryAppService inventoryService, IFileAppService fileAppService)
    {
        _context = context;
        _inventoryService = inventoryService;
        _fileAppService = fileAppService;
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto input)
    {
        // 1. Creamos la entidad maestra del Producto
        var product = new Product
        {
            Name = input.Name,
            Description = input.Description,
            SKU = input.SKU,
            Barcode = input.Barcode,
            ReferencePrice = input.ReferencePrice
        };

        _context.Products.Add(product);

        // Guardamos para obtener el Id del producto antes de crear el lote
        await _context.SaveChangesAsync();

        // 2. Registramos el lote inicial usando el servicio de inventario
        await _inventoryService.AddStockAsync(product.Id, input.InitialBatch);

        return await GetByIdAsync(product.Id);
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Batches)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) throw new UserFriendlyException("Producto no encontrado.");

        return MapToResponseDto(product);
    }

    public async Task<List<ProductResponseDto>> GetListAsync()
    {
        var products = await _context.Products
            .Include(p => p.Batches)
            .ToListAsync();

        return products.Select(MapToResponseDto).ToList();
    }

    public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto input)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) throw new UserFriendlyException("Producto no encontrado.");

        var imageUrl = await _fileAppService.SaveFileAsync(input.ImageUrl!, "products");

        product.Name = input.Name;
        product.Description = input.Description;
        product.SKU = input.SKU;
        product.Barcode = input.Barcode;
        product.ImageUrl = imageUrl;
        product.ReferencePrice = input.ReferencePrice;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<ProductResponseDto> DeleteAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Batches)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            throw new UserFriendlyException($"El producto con ID {id} no existe o ya fue eliminado.");
        }

        if (product.TotalStock > 0)
        {
            throw new UserFriendlyException(
                $"No se puede eliminar el producto '{product.Name}' porque aún tiene {product.TotalStock} unidades en stock distribuidas en sus lotes.");
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return MapToResponseDto(product);
    }

    private ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU ?? "",
            ReferencePrice = product.ReferencePrice,
            ImageUrl = product.ImageUrl,
            TotalStock = product.Batches.Sum(b => b.RemainingQuantity),
            ActiveBatches = product.Batches
                .Where(b => b.RemainingQuantity > 0)
                .Select(b => new BatchResponseDto
                {
                    Id = b.Id,
                    BatchNumber = b.BatchNumber,
                    RemainingQuantity = b.RemainingQuantity,
                    SalePrice = b.SalePrice,
                    DiscountThreshold = b.DiscountThreshold,
                    DiscountAmount = b.DiscountAmount
                }).ToList()
        };
    }
}