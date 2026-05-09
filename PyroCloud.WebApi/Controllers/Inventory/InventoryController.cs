using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Inventory.Batchs;
using PyroCloud.Modules.Inventory.Interfaces;

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

        // Registrar una nueva compra (nuevo lote)
        [HttpPost("{productId}/add-batch")]
        public async Task<ActionResult<BatchResponseDto>> AddBatch(Guid productId, [FromBody] CreateBatchDto input)
        {
            var batch = await _inventoryService.AddStockAsync(productId, input);
            return Ok(batch);
        }

        // Consultar lotes activos de un producto
        [HttpGet("{productId}/batches")]
        public async Task<ActionResult<List<BatchResponseDto>>> GetActiveBatches(Guid productId)
        {
            var batches = await _inventoryService.GetActiveBatchesAsync(productId);
            return Ok(batches);
        }

        // Consultar precio unitario según cantidad (Regla de negocio de lotes)
        [HttpGet("{productId}/price")]
        public async Task<ActionResult<decimal>> GetUnitPrice(Guid productId, [FromQuery] int quantity)
        {
            var price = await _inventoryService.GetUnitPriceAsync(productId, quantity);
            return Ok(price);
        }
    }
}
