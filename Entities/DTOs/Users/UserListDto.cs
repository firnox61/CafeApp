using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Users
{
    public class UserListDto:IDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } // FirstName + LastName birleştirilmiş
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
