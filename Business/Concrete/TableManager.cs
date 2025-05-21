using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Orders;
using Entities.DTOs.Products;
using Entities.DTOs.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TableManager : ITableService
    {
        private readonly ITableDal _tableDal;
        private readonly IMapper _mapper;

        public TableManager(ITableDal tableDal, IMapper mapper)
        {
            _tableDal = tableDal;
            _mapper = mapper;
        }

        public async Task<IResult> Add(TableCreateDto tableCreateDto)
        {
            var newTable = _mapper.Map<Table>(tableCreateDto);
            await _tableDal.AddAsync(newTable);
            return new SuccessResult();
        }

        public async Task<IResult> Delete(Table table)
        {
           await _tableDal.DeleteAsync(table);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<TableGetDto>>> GetAllAsync()
        {
            var tables = await _tableDal.GetAllWithOrdersAsync();

            var dtos = tables.Select(table => new TableGetDto
            {
                Id = table.Id,
                Name = table.Name,
                ActiveOrders = table.Orders
                    .Where(o => !o.IsPaid)
                    .Select(o => new OrderSummaryDto
                    {
                        OrderId = o.Id,
                        CreatedAt = o.CreatedAt,
                        TotalAmount = o.OrderItems.Sum(i => i.Quantity * i.UnitPrice)
                    }).ToList()
            }).ToList();

            return new SuccessDataResult<List<TableGetDto>>(dtos);
        }


        public async Task<IDataResult<TableGetDto?>> GetById(int id)
        {
            var table = await _tableDal.GetWithOrdersByIdAsync(id);
            if (table == null)
                return new ErrorDataResult<TableGetDto?>("Masa bulunamadı.");

            var dto = _mapper.Map<TableGetDto>(table);
            return new SuccessDataResult<TableGetDto?>(dto);
        }

        public async Task<IResult> Update(TableUpdateDto tableUpdateDto)
        {

           var updateTable= _mapper.Map<Table>(tableUpdateDto);
            await _tableDal.UpdateAsync(updateTable);
            return new SuccessResult();
        }
    }
}
