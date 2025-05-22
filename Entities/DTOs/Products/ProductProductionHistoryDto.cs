using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductProductionHistoryDto
    {
        public string ProductName { get; set; } = null!;
        public int TotalProduced { get; set; }
        public DateTime LastProducedAt { get; set; }
    }

}
