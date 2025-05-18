using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Table : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // Örn: Masa 1

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
