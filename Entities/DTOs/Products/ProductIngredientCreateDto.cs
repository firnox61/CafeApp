using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductIngredientCreateDto : IDto
    {
       // public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public double QuantityRequired { get; set; }
    }
}
