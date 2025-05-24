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
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OperationClaimCreateDto dto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto dto); // ✅ güncel hali
        Task<IResult> DeleteAsync(int id);
    }
}
