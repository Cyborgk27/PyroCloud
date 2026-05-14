using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Inventory.Batchs;
using PyroCloud.Modules.Inventory.Authorization;
using PyroCloud.Modules.Inventory.Interfaces;
using PyroCloud.Shared.Infrastructure.Authorization;

namespace PyroCloud.WebApi.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryAppService _inventoryService;

        public InventoryController(IInventoryAppService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("{productId}/add-batch")]
        [HasPermission(InventoryPermissions.Stock.AddBatch)]
        public async Task<ActionResult<BatchResponseDto>> AddBatch(Guid productId, [FromBody] CreateBatchDto input)
        {
            var batch = await _inventoryService.AddStockAsync(productId, input);
            return Ok(batch);
        }

        [HttpGet("{productId}/batches")]
        [HasPermission(InventoryPermissions.Stock.Default)]
        public async Task<ActionResult<List<BatchResponseDto>>> GetActiveBatches(Guid productId)
        {
            var batches = await _inventoryService.GetActiveBatchesAsync(productId);
            return Ok(batches);
        }

        [HttpGet("{productId}/price")]
        [HasPermission(InventoryPermissions.Stock.ViewPrices)]
        public async Task<ActionResult<decimal>> GetUnitPrice(Guid productId, [FromQuery] int quantity)
        {
            var price = await _inventoryService.GetUnitPriceAsync(productId, quantity);
            return Ok(price);
        }
    }
}