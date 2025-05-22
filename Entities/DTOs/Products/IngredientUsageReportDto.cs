using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class IngredientUsageReportDto
    {
        public string IngredientName { get; set; } = null!;
        public double TotalUsedAmount { get; set; }
    }
}
