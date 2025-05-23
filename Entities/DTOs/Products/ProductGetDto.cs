using Core;
using Entities.DTOs.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductGetDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageFileName { get; set; }

        //public List<ProductIngredientGetDto> ProductIngredients { get; set; } = new();
        public List<IngredientDto> Ingredients { get; set; } = new();
    }
}
