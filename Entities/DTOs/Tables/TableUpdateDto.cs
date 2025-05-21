using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Tables
{
    public class TableUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // Örn: Masa 1
    }
}
