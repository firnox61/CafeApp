using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Products
{
    public class ProductProductionReportDto
    {
        public string ProductName { get; set; } = null!;
        public int TotalProduced { get; set; }
    }
}
