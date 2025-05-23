﻿using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderDal:IEntityRepository<Order>
    {
        Task<List<Order>> GetAllWithDetailsAsync();
        Task<Order?> GetOrderWithDetailsAsync(int id);
        Task<Order?> GetWithItemsAsync(int orderId);

    }
}
