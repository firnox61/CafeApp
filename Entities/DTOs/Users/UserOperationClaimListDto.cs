using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Users
{
    public class UserOperationClaimListDto:IDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }
        public string RoleName { get; set; }
    }
}
