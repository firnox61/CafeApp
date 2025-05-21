using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Orders
{
    public class OrderItemsCreateDto : IDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
