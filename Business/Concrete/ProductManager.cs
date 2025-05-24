using AutoMapper;
using Business.Abstract;
using Core;
using Core.Aspects.Autofac.Transaction;
using Core.DataAccess;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.AspNetCore.Http;
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
        IFileService _fileService;

        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal, IMapper mapper, IProductIngredientDal productIngredientDal, IIngredientDal ingredientDal,
            IProductionHistoryDal productionHistoryDal, IFileService fileService)
        {
            _productDal = productDal;
            _mapper = mapper;
            _productIngredientDal = productIngredientDal;
            _ingredientDal = ingredientDal;
            _productionHistoryDal = productionHistoryDal;
            _fileService = fileService; 
        }
        // [TransactionScopeAspect]
        public async Task<IResult> Add(ProductCreateDto productCreateDto)
        {
            // 1. Malzeme stok kontrolü
            foreach (var item in productCreateDto.ProductIngredients!)
            {
                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Malzeme bulunamadı. ID: {item.IngredientId}");

                var required = item.QuantityRequired * productCreateDto.Stock;
                if (ingredient.Stock < required)
                    return new ErrorResult($"Yetersiz stok: {ingredient.Name}. Gerekli: {required}, Mevcut: {ingredient.Stock}");
            }

            // 2. Görsel yükle
            var fileName = await _fileService.UploadImageAsync(productCreateDto.Image, "images/products");

            // 3. Ürün oluştur
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,
                CreatedAt = DateTime.UtcNow,
                ImageFileName = fileName,
            };

            await _productDal.AddAsync(product);

            // 4. Üretim geçmişi kaydı
            await _productionHistoryDal.AddAsync(new ProductionHistory
            {
                ProductId = product.Id,
                QuantityProduced = product.Stock,
                ProducedAt = DateTime.UtcNow
            });

            // 5. Malzeme ilişkisi ve stok düşümü
            foreach (var item in productCreateDto.ProductIngredients!)
            {
                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Stok güncellemede malzeme eksik: {item.IngredientId}");

                // Malzeme ilişkisi ekle
                await _productIngredientDal.AddAsync(new ProductIngredient
                {
                    ProductId = product.Id,
                    IngredientId = item.IngredientId,
                    QuantityRequired = item.QuantityRequired
                });

                // Stok güncelle
                ingredient.Stock -= item.QuantityRequired * product.Stock;
                await _ingredientDal.UpdateAsync(ingredient);

                // Kritik stok uyarısı
                if (ingredient.Stock <= ingredient.MinStockThreshold)
                {
                    Console.WriteLine($"⚠️ Kritik stok: {ingredient.Name} | Mevcut: {ingredient.Stock}, Eşik: {ingredient.MinStockThreshold}");
                }
            }

            return new SuccessResult("Ürün ve malzemeleri başarıyla eklendi.");
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

        // [TransactionScopeAspect]
        public async Task<IResult> Update(ProductUpdateDto productUpdateDto)
        {
            var product = await _productDal.GetAsync(p => p.Id == productUpdateDto.Id);
            if (product == null)
                return new ErrorResult("Ürün bulunamadı.");

            // Görsel güncellemesi varsa
            if (productUpdateDto.Image != null)
            {
                await _fileService.DeleteImageAsync("images/products", product.ImageFileName!);
                var newImageName = await _fileService.UploadImageAsync(productUpdateDto.Image, "images/products");
                product.ImageFileName = newImageName;
            }

            // Diğer alanları güncelle
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;

            if (productUpdateDto.Stock != product.Stock)
            {
                var diff = productUpdateDto.Stock - product.Stock;
                product.Stock = productUpdateDto.Stock;

                var history = new ProductionHistory
                {
                    ProductId = product.Id,
                    QuantityProduced = diff,
                    ProducedAt = DateTime.UtcNow
                };
                await _productionHistoryDal.AddAsync(history);
            }

            await _productDal.UpdateAsync(product);

            // 🔥 Ingredient ilişkilerini güncelle
            var existingRelations = await _productIngredientDal.GetAllAsync(pi => pi.ProductId == product.Id);
            foreach (var relation in existingRelations)
            {
                await _productIngredientDal.DeleteAsync(relation);
            }

            foreach (var item in productUpdateDto.ProductIngredients!)
            {
                await _productIngredientDal.AddAsync(new ProductIngredient
                {
                    ProductId = product.Id,
                    IngredientId = item.IngredientId,
                    QuantityRequired = item.QuantityRequired
                });

                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Malzeme bulunamadı (ID: {item.IngredientId})");

                ingredient.Stock -= item.QuantityRequired * product.Stock;
                await _ingredientDal.UpdateAsync(ingredient);

                if (ingredient.Stock <= ingredient.MinStockThreshold)
                {
                    Console.WriteLine($"⚠️ Kritik stok: {ingredient.Name} | Mevcut: {ingredient.Stock}, Eşik: {ingredient.MinStockThreshold}");
                }
            }

            return new SuccessResult("Ürün ve malzemeleri başarıyla güncellendi.");
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
        private async Task<IResult> CheckIngredientStocksAsync(List<ProductIngredientCreateDto> ingredients, int stockAmount)
        {
            foreach (var item in ingredients)
            {
                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Malzeme bulunamadı: ID {item.IngredientId}");

                var required = item.QuantityRequired * stockAmount;
                if (ingredient.Stock < required)
                    return new ErrorResult($"Yetersiz stok: {ingredient.Name}. Gerekli: {required}, Mevcut: {ingredient.Stock}");
            }
            return new SuccessResult();
        }
        private async Task<IDataResult<string?>> UploadImageAsync(IFormFile? image)
        {
            var fileName = await _fileService.UploadImageAsync(image, "images/products");
            return new SuccessDataResult<string?>(fileName);
        }
        private Product CreateProductEntity(ProductCreateDto dto, string? fileName)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedAt = DateTime.UtcNow,
                ImageFileName = fileName
            };
        }
        private async Task LogProductionAsync(Product product)
        {
            var history = new ProductionHistory
            {
                ProductId = product.Id,
                QuantityProduced = product.Stock,
                ProducedAt = DateTime.UtcNow
            };
            await _productionHistoryDal.AddAsync(history);
        }
        private async Task<IResult> SaveIngredientRelationsAndUpdateStocksAsync(List<ProductIngredientCreateDto> ingredients, int stockAmount, int productId)
        {
            foreach (var item in ingredients)
            {
                await _productIngredientDal.AddAsync(new ProductIngredient
                {
                    ProductId = productId,
                    IngredientId = item.IngredientId,
                    QuantityRequired = item.QuantityRequired
                });

                var ingredient = await _ingredientDal.GetAsync(i => i.Id == item.IngredientId);
                if (ingredient == null)
                    return new ErrorResult($"Malzeme bulunamadı (stok güncelleme): ID {item.IngredientId}");

                ingredient.Stock -= item.QuantityRequired * stockAmount;
                await _ingredientDal.UpdateAsync(ingredient);

                if (ingredient.Stock <= ingredient.MinStockThreshold)
                {
                    Console.WriteLine($"⚠️ Kritik stok: {ingredient.Name} | Mevcut: {ingredient.Stock}, Eşik: {ingredient.MinStockThreshold}");
                }
            }
            return new SuccessResult();
        }
    }

}
