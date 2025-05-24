using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Ingredients
{
    public class StockAlertDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public double Stock { get; set; }
        public double MinStockThreshold { get; set; }
        public bool IsCritical => Stock <= MinStockThreshold;
    }
}
