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
    public class EfTableDal: EfEntityRepositoryBase<Table, DataContext>, ITableDal
    {
        public EfTableDal(DataContext context) : base(context) { }
        public async Task<List<Table>> GetAllWithOrdersAsync()
        {
            return await _context.Tables
                .Include(t => t.Orders)
                    .ThenInclude(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Table?> GetWithOrdersByIdAsync(int id)
        {
            return await _context.Tables
                .Include(t => t.Orders)
                    .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

    }
}
