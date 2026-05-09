using PyroCloud.Core.Application.Dtos.Inventory.Batchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroCloud.Core.Application.Dtos.Inventory.Products
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public decimal ReferencePrice { get; set; }
        public int TotalStock { get; set; }
        public string? ImageUrl { get; set; }
        public List<BatchResponseDto> ActiveBatches { get; set; } = new();
    }
}
