﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Users
{
    public class UserOperationClaimCreateDto
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
