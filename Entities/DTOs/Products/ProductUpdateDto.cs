using Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductUpdateDto : IDto
    {
        public int Id { get; set; } // Güncellenecek ürün ID'si
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile? Image { get; set; }

        // Opsiyonel olarak malzeme güncellemesi de desteklenebilir
        public List<ProductIngredientCreateDto>? ProductIngredients { get; set; } = new();
    }

}
