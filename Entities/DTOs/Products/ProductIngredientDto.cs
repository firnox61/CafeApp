using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductIngredientDto:IDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public double QuantityRequired { get; set; }
    }
}
