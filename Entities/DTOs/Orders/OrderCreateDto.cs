using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Orders
{
    public class OrderCreateDto : IDto
    {
        public int TableId { get; set; }
        public List<OrderItemsCreateDto> Items { get; set; } = new();
    }
}
