using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Dtos.Inventory;
using PyroCloud.Core.Application.Dtos.Inventory.Batchs;
using PyroCloud.Core.Domain.Entities.Inventory;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Modules.Inventory.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Inventory.Services;

public class InventoryAppService : IInventoryAppService
{
    private readonly PyroDbContext _context;

    public InventoryAppService(PyroDbContext context)
    {
        _context = context;
    }

    public async Task<BatchResponseDto> AddStockAsync(Guid productId, CreateBatchDto input)
    {
        var batch = new ProductBatch
        {
            ProductId = productId,
            BatchNumber = input.BatchNumber,
            InitialQuantity = input.Quantity,
            RemainingQuantity = input.Quantity,
            PurchasePrice = input.PurchasePrice,
            SalePrice = input.SalePrice,
            DiscountThreshold = input.DiscountThreshold,
            DiscountAmount = input.DiscountAmount,
            ExpiryDate = input.ExpiryDate,
            ReceptionDate = DateTime.Now
        };

        _context.ProductBatches.Add(batch);
        await _context.SaveChangesAsync();

        return new BatchResponseDto
        {
            Id = batch.Id,
            BatchNumber = batch.BatchNumber,
            RemainingQuantity = batch.RemainingQuantity,
            SalePrice = batch.SalePrice,
            DiscountThreshold = batch.DiscountThreshold,
            DiscountAmount = batch.DiscountAmount
        };
    }

    public async Task<List<BatchResponseDto>> GetActiveBatchesAsync(Guid productId)
    {
        return await _context.ProductBatches
            .Where(b => b.ProductId == productId && b.RemainingQuantity > 0)
            .OrderBy(b => b.ReceptionDate) // Orden por fecha para facilitar FIFO
            .Select(b => new BatchResponseDto
            {
                Id = b.Id,
                BatchNumber = b.BatchNumber,
                RemainingQuantity = b.RemainingQuantity,
                SalePrice = b.SalePrice,
                DiscountThreshold = b.DiscountThreshold,
                DiscountAmount = b.DiscountAmount
            }).ToListAsync();
    }

    public async Task<decimal> GetUnitPriceAsync(Guid productId, int requestedQuantity)
    {
        // Buscamos el lote más antiguo con stock disponible
        var batch = await _context.ProductBatches
            .Where(b => b.ProductId == productId && b.RemainingQuantity > 0)
            .OrderBy(b => b.ReceptionDate)
            .FirstOrDefaultAsync();

        if (batch == null) throw new Exception("No hay stock disponible para este producto.");

        // Aplicamos la regla de negocio del Batch
        return batch.CalculateUnitPrice(requestedQuantity);
    }

    public async Task DecreaseStockAsync(Guid productId, int quantity)
    {
        var remainingToDecrease = quantity;

        // Obtenemos lotes con stock ordenados por fecha (FIFO)
        var batches = await _context.ProductBatches
            .Where(b => b.ProductId == productId && b.RemainingQuantity > 0)
            .OrderBy(b => b.ReceptionDate)
            .ToListAsync();

        if (batches.Sum(b => b.RemainingQuantity) < quantity)
            throw new UserFriendlyException("Stock total insuficiente para cubrir la demanda.");

        foreach (var batch in batches)
        {
            if (remainingToDecrease <= 0) break;

            if (batch.RemainingQuantity >= remainingToDecrease)
            {
                batch.ReduceStock(remainingToDecrease);
                remainingToDecrease = 0;
            }
            else
            {
                remainingToDecrease -= batch.RemainingQuantity;
                batch.ReduceStock(batch.RemainingQuantity);
            }
        }

        await _context.SaveChangesAsync();
    }
}