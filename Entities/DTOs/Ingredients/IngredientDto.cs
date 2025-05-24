using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Ingredients
{
    public class IngredientDto:IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Stock { get; set; }
        public decimal MinStockThreshold { get; set; }
    }
}
