using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<List<UserOperationClaimListDto>>> GetByUserIdAsync(int userId);
        Task<IResult> AddAsync(UserOperationClaimCreateDto dto);
        Task<IResult> DeleteAsync(int id);
    }
}
