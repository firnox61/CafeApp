using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderItemService
    {
        Task<IDataResult<List<OrderItem>>> GetAllAsync();
        Task<IDataResult<OrderItem?>> GetById(int id);
        Task<IResult> Add(OrderItemsCreateDto orderItemsCreateDto);
        Task<IResult> Update(OrderItem orderItem);
        Task<IResult> Delete(OrderItem orderItem);
    }
}
