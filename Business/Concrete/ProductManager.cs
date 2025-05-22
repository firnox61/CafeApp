using AutoMapper;
using Business.Abstract;
using Core.DataAccess;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
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
        private readonly IIngredientDal _ingredientDal;
        private readonly IProductionHistoryDal _productionHistoryDal;

        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal, IMapper mapper, IProductIngredientDal productIngredientDal, IIngredientDal ingredientDal, IProductionHistoryDal productionHistoryDal)
        {
            _productDal = productDal;
            _mapper=mapper;
            _productIngredientDal = productIngredientDal;
            _ingredientDal=ingredientDal;
            _productionHistoryDal=productionHistoryDal;
        }

        public async Task<IResult> Add(ProductCreateDto productCreateDto)
        {
            // 1. Malzeme stoklarını kontrol et
            foreach (var item in productCreateDto.ProductIngredients!)
            {
                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Malzeme bulunamadı. ID: {item.IngredientId}");

                var required = item.QuantityRequired * productCreateDto.Stock;

                if (ingredient.Stock < required)
                    return new ErrorResult($"Yetersiz stok: {ingredient.Name}. Gerekli: {required}, Mevcut: {ingredient.Stock}");
            }

            // 2. Ürün kaydı
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,
                CreatedAt= DateTime.UtcNow,
            };

            await _productDal.AddAsync(product);
            // ✅ Üretim geçmişi kaydı oluştur
            var productionHistory = new ProductionHistory
            {
                ProductId = product.Id,
                QuantityProduced = product.Stock,
                ProducedAt = DateTime.UtcNow
            };
            await _productionHistoryDal.AddAsync(productionHistory);

            // 3. Malzeme ilişkisi ve stok düşümü
            foreach (var item in productCreateDto.ProductIngredients!)
            {
                var relation = new ProductIngredient
                {
                    ProductId = product.Id,
                    IngredientId = item.IngredientId,
                    QuantityRequired = item.QuantityRequired
                };
                await _productIngredientDal.AddAsync(relation);

                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                ingredient.Stock -= item.QuantityRequired * productCreateDto.Stock;
                await _ingredientDal.UpdateAsync(ingredient);
                if (ingredient.Stock <= ingredient.MinStockThreshold)
                {
                    // Şu anlık loglayabiliriz, ileride mail / notification sistemi ile genişletilebilir
                    Console.WriteLine($"⚠️ DİKKAT: {ingredient.Name} stoğu kritik seviyeye düştü! Mevcut: {ingredient.Stock}, Eşik: {ingredient.MinStockThreshold}");
                }
            }

            return new SuccessResult("Ürün ve malzemeleri başarıyla eklendi, stoklar güncellendi.");
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
        public async Task<IDataResult<List<ProductProductionReportDto>>> GetMostProducedProductsAsync()
        {
            var products = await _productDal.GetAllAsync();

            var result = products
                .Select(p => new ProductProductionReportDto
                {
                    ProductName = p.Name,
                    TotalProduced = p.Stock
                })
                .OrderByDescending(p => p.TotalProduced)
                .ToList();

            return new SuccessDataResult<List<ProductProductionReportDto>>(result);
        }
        public async Task<IDataResult<List<ProductProductionHistoryDto>>> GetProductionHistoryReportAsync()
        {
            var records = await _productionHistoryDal.GetAllWithProductAsync();

            var report = records
                .GroupBy(p => p.Product.Name)
                .Select(g => new ProductProductionHistoryDto
                {
                    ProductName = g.Key,
                    TotalProduced = g.Sum(x => x.QuantityProduced),
                    LastProducedAt = g.Max(x => x.ProducedAt)
                })
                .OrderByDescending(x => x.TotalProduced)
                .ToList();

            return new SuccessDataResult<List<ProductProductionHistoryDto>>(report);
        }

    }
}
