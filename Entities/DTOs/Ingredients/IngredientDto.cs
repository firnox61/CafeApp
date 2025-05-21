using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Ingredients
{
    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = null!;
        public double QuantityRequired { get; set; }
    }
}
