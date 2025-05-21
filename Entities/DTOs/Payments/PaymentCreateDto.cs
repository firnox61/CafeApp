using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Payments
{
    public class PaymentCreateDto : IDto
    {
        public int OrderId { get; set; }
      //  public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; } = null!; // "Nakit", "Kredi Kartı" vs.

    }
}
