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
    public interface IOrderService
    {
        Task<IDataResult<List<OrderGetDto>>> GetAllAsync();
        Task<IDataResult<OrderGetDto>> GetById(int id);
        Task<IResult> Add(OrderCreateDto orderCreateDto);
        Task<IResult> Update(Order order);
        Task<IResult> Delete(Order order);
    }
}
