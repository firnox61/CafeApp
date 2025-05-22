using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductionHistory : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityProduced { get; set; }
        public DateTime ProducedAt { get; set; }

        public Product Product { get; set; } = null!;
    }
}
