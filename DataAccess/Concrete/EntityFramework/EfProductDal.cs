using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.DTOs.Ingredients;
using Entities.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, DataContext>, IProductDal
    {
        public EfProductDal(DataContext context) : base(context) { }
        public async Task<List<ProductGetDto>> GetProductDetailsAsync()
        {
            var result = await _context.Products
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Select(p => new ProductGetDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Ingredients = p.ProductIngredients.Select(pi => new IngredientDto
                    {
                        IngredientId = pi.IngredientId,
                        IngredientName = pi.Ingredient.Name,
                        QuantityRequired = pi.QuantityRequired
                    }).ToList()
                })
                .ToListAsync();

            return result;
        }
    

    }
}
