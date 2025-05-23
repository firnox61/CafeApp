using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MinStockThreshold { get; set; } = 0; // ✅ Kritik stok eşiği
        public DateTime CreatedAt { get; set; }
        public string? ImageFileName { get; set; }
        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
        public ICollection<ProductionHistory> ProductionHistories { get; set; } = new List<ProductionHistory>();

    }
}
