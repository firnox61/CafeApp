using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPaymentService
    {
        Task<IDataResult<List<PaymentGetDto>>> GetAllAsync();
        Task<IDataResult<PaymentGetDto?>> GetById(int id);
        Task<IResult> Add(PaymentCreateDto paymentCreateDto);
        Task<IResult> Update(Payment payment);
        Task<IResult> Delete(Payment payment);
    }
}
