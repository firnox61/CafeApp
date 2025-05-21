using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly IPaymentDal _paymentDal;
        private readonly IOrderDal _orderDal;
        private readonly IMapper _mapper;
        public PaymentManager(IPaymentDal paymentDal, IMapper mapper, IOrderDal orderDal)
        {
            _paymentDal = paymentDal;
            _mapper = mapper;
            _orderDal = orderDal;
        }

        public async Task<IResult> Add(PaymentCreateDto paymentCreateDto)
        {
            var order = await _orderDal.GetWithItemsAsync(paymentCreateDto.OrderId);

            if (order == null)
                return new ErrorResult("Sipariş bulunamadı.");

            if (order.IsPaid)
                return new ErrorResult("Bu sipariş zaten ödenmiş.");

            // Toplamı sistem hesaplasın
            var totalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentType = paymentCreateDto.PaymentType,
                PaidAt = DateTime.Now,
                TotalAmount = totalAmount
            };

            order.IsPaid = true;
            await _paymentDal.AddAsync(payment);

            return new SuccessResult("Ödeme başarıyla tamamlandı.");
        }




        public async Task<IResult> Delete(Payment payment)
        {
            await _paymentDal.DeleteAsync(payment);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<PaymentGetDto>>> GetAllAsync()
        {
            var payments = await _paymentDal.GetAllAsync();
            var dtos = _mapper.Map<List<PaymentGetDto>>(payments);
            return new SuccessDataResult<List<PaymentGetDto>>(dtos);
        }

        public async Task<IDataResult<PaymentGetDto?>> GetById(int id)
        {
            var payment = await _paymentDal.GetAsync(p => p.Id == id);

            if (payment == null)
                return new ErrorDataResult<PaymentGetDto?>("Payment not found");

            var getPayment = _mapper.Map<PaymentGetDto>(payment);
            return new SuccessDataResult<PaymentGetDto?>(getPayment);
        }

        public async Task<IResult> Update(Payment payment)
        {
            await _paymentDal.UpdateAsync(payment);
            return new SuccessResult();
        }
    }
}
