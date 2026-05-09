using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Inventory.Products;
using PyroCloud.Modules.Inventory.Interfaces;

namespace PyroCloud.WebApi.Controllers.Inventory
{
    [Route("api/inventory/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> GetList()
        {
            var products = await _productAppService.GetListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
        {
            var product = await _productAppService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductDto input)
        {
            var product = await _productAppService.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDto>> Update(Guid id, [FromBody] UpdateProductDto input)
        {
            var product = await _productAppService.UpdateAsync(id, input);
            return Ok(product);
        }
    }
}
