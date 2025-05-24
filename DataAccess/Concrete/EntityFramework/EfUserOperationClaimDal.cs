using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal:EfEntityRepositoryBase<UserOperationClaim,DataContext>, IUserOperationClaimDal
    {
        public EfUserOperationClaimDal(DataContext context) : base(context) { }

        public async Task<List<UserOperationClaim>> GetWithDetailsByUserIdAsync(int userId)
        {
            return await _context.UserOperationClaims
                .Include(x => x.User)
                .Include(x => x.OperationClaim)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
