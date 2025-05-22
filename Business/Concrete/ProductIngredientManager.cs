using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using Entities.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductIngredientManager : IProductIngredientService
    {
        private readonly IProductIngredientDal _productIngredientDal;
        private readonly IMapper _mapper;

        public ProductIngredientManager(IProductIngredientDal productIngredientDal, IMapper mapper)
        {
            _productIngredientDal = productIngredientDal;
            _mapper = mapper;
        }

        public async Task<IResult> Add(ProductIngredientCreateDto productIngredientCreateDto)
        {
            var newProIngrediton=_mapper.Map<ProductIngredient>(productIngredientCreateDto);
            await _productIngredientDal.AddAsync(newProIngrediton);
            return new SuccessResult();
        }

        public async Task<IResult> Delete(ProductIngredient productIngredient)
        {
          await _productIngredientDal.DeleteAsync(productIngredient);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<ProductIngredientGetDto>>> GetAllAsync()
        {
            var result = await _productIngredientDal.GetAllWithDetailsAsync();
            return new SuccessDataResult<List<ProductIngredientGetDto>>(result);
        }

        public async Task<IDataResult<ProductIngredientGetDto>> GetById(int id)
        {
            var result = await _productIngredientDal.GetByIdWithDetailsAsync(id);
            if (result == null)
                return new ErrorDataResult<ProductIngredientGetDto>("Veri bulunamadı.");

            return new SuccessDataResult<ProductIngredientGetDto>(result);
        }
        public async Task<IDataResult<List<IngredientUsageReportDto>>> GetMostUsedIngredientsAsync()
        {
            var productIngredients = await _productIngredientDal.GetAllWithProductAndIngredientAsync();

            var result = productIngredients
                .GroupBy(pi => pi.Ingredient.Name)
                .Select(g => new IngredientUsageReportDto
                {
                    IngredientName = g.Key,
                    TotalUsedAmount = g.Sum(pi => pi.QuantityRequired * pi.Product.Stock)
                })
                .OrderByDescending(x => x.TotalUsedAmount)
                .ToList();

            return new SuccessDataResult<List<IngredientUsageReportDto>>(result);
        }
        public async Task<IDataResult<List<IngredientUsageReportDto>>> GetMostUsedIngredientsAsync(UsageReportFilterDto filter)
        {
            var productIngredients = await _productIngredientDal.GetAllWithProductAndIngredientAsync();

            if (filter.StartDate.HasValue)
                productIngredients = productIngredients
                    .Where(pi => pi.Product.CreatedAt >= filter.StartDate.Value)
                    .ToList();

            if (filter.EndDate.HasValue)
                productIngredients = productIngredients
                    .Where(pi => pi.Product.CreatedAt <= filter.EndDate.Value)
                    .ToList();

            var result = productIngredients
                .GroupBy(pi => pi.Ingredient.Name)
                .Select(g => new IngredientUsageReportDto
                {
                    IngredientName = g.Key,
                    TotalUsedAmount = g.Sum(pi => pi.QuantityRequired * pi.Product.Stock)
                })
                .OrderByDescending(x => x.TotalUsedAmount)
                .ToList();

            return new SuccessDataResult<List<IngredientUsageReportDto>>(result);
        }

    }
}
