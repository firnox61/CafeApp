using AutoMapper;
using Business.Abstract;
using Core.DataAccess;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductIngredientDal _productIngredientDal;

        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal, IMapper mapper, IProductIngredientDal productIngredientDal)
        {
            _productDal = productDal;
            _mapper=mapper;
            _productIngredientDal = productIngredientDal;
        }

        public async Task<IResult> Add(ProductCreateDto productCreateDto)
        {
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock
            };

            // Önce ürünü ekle ki Id oluşsun
            await _productDal.AddAsync(product);

            // Ingredient ilişkilerini kur
            if (productCreateDto.ProductIngredients != null)
            {
                foreach (var item in productCreateDto.ProductIngredients)
                {
                    var relation = new ProductIngredient
                    {
                        ProductId = product.Id, // Artık oluştu
                        IngredientId = item.IngredientId,
                        QuantityRequired = item.QuantityRequired
                    };
                    await _productIngredientDal.AddAsync(relation);
                }
            }

            return new SuccessResult("Ürün ve içerikleri başarıyla eklendi.");
        }



        public async Task<IResult> Delete(int productId)
        {
            var product = await _productDal.GetAsync(p => p.Id == productId);
            if (product == null)
                return new ErrorResult("Ürün bulunamadı.");

            await _productDal.DeleteAsync(product);
            return new SuccessResult("Ürün silindi.");
        }

        public async Task<IDataResult<List<ProductGetDto>>> GetAllAsync()
        {
            var result = await _productDal.GetProductDetailsAsync(); // Bu zaten senkron bir metod

            return new SuccessDataResult<List<ProductGetDto>>(result);

            // return new SuccessDataResult<List<Product>>(await _productDal.GetAllAsync());
        }

        public async Task<IDataResult<Product?>> GetById(int id)
        {
            await _productDal.GetAsync(p=>p.Id == id);
            return new SuccessDataResult<Product?>();
        }

        public async Task<IResult> Update(Product product)
        {
            await _productDal.UpdateAsync(product);
            return new SuccessResult();
        }
    }
}
