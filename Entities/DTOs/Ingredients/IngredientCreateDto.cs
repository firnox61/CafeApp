using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Ingredients
{
    public class IngredientCreateDto:IDto
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!; // örn: gram, ml, adet
        public double Stock { get; set; }
    }
}
