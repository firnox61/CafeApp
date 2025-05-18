using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductIngredient : IEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public double QuantityRequired { get; set; } // 1 ürün için gereken miktar
    }
}
