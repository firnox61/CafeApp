using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderItemDal _orderItemDal;
        private readonly IProductDal _productDal; 
        private readonly IMapper _mapper;


        public OrderManager(IOrderDal orderDal, IMapper mapper, IProductDal productDal, IOrderItemDal orderItemDal)
        {
            _orderDal = orderDal;
            _mapper = mapper;
            _productDal = productDal;
            _orderItemDal = orderItemDal;
        }

        public async Task<IResult> Add(OrderCreateDto orderCreateDto)
        {
            // 1. Order nesnesini oluştur
            var newOrder = new Order
            {
                TableId = orderCreateDto.TableId,
                CreatedAt = DateTime.Now,
                IsPaid = false
            };

            // 2. Önce veritabanına yaz ki Id oluşsun
            await _orderDal.AddAsync(newOrder); // içinde SaveChangesAsync() olmalı

            // 3. Ürünleri bul ve OrderItems olarak kaydet
            foreach (var item in orderCreateDto.Items)
            {
                var product = await _productDal.GetAsync(p => p.Id == item.ProductId);
                if (product == null) continue;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    OrderId = newOrder.Id // 🔥 artık OrderId var
                };

                await _orderItemDal.AddAsync(orderItem); // OrderItem ayrı ekleniyor
            }

            return new SuccessResult("Sipariş başarıyla eklendi.");
        }


        public async Task<IResult> Delete(Order order)
        {
            await _orderDal.DeleteAsync(order);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<OrderGetDto>>> GetAllAsync()
        {
            var orders = await _orderDal.GetAllWithDetailsAsync();

            // AutoMapper ile Entity -> DTO dönüşümü
            var orderDtos = _mapper.Map<List<OrderGetDto>>(orders);

            return new SuccessDataResult<List<OrderGetDto>>(orderDtos);
          
        }

        public async Task<IDataResult<OrderGetDto>> GetById(int id)
        {

            var order = await _orderDal.GetOrderWithDetailsAsync(id);

            if (order == null)
                return new ErrorDataResult<OrderGetDto>("Sipariş bulunamadı.");

            var dto = _mapper.Map<OrderGetDto>(order);
            return new SuccessDataResult<OrderGetDto>(dto);
        }

        public async Task<IResult> Update(Order order)
        {
           await _orderDal.UpdateAsync(order);
            return new SuccessResult();
        }
    }
}
