using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductIngredientDal:EfEntityRepositoryBase<ProductIngredient, DataContext>,IProductIngredientDal
    {
        public EfProductIngredientDal(DataContext context) : base(context) { }
        /* public async Task<List<ProductIngredient>> GetAllWithIncludesAsync()
         {
             return await _context.ProductIngredients
                 .Include(pi => pi.Product)
                 .Include(pi => pi.Ingredient)
                 .ToListAsync();
         }*/
        public async Task<List<ProductIngredientGetDto>> GetAllWithDetailsAsync()
        {
            return await _context.ProductIngredients
                .Include(pi => pi.Ingredient)
                .Select(pi => new ProductIngredientGetDto
                {
                    ProductId = pi.ProductId,
                    IngredientId = pi.IngredientId,
                    IngredientName = pi.Ingredient.Name,
                    QuantityRequired = pi.QuantityRequired
                })
                .ToListAsync();
        }
        public async Task<ProductIngredientGetDto?> GetByIdWithDetailsAsync(int ingredientId)
        {
            return await _context.ProductIngredients
                .Include(pi => pi.Ingredient)
                .Where(pi => pi.IngredientId == ingredientId)
                .Select(pi => new ProductIngredientGetDto
                {
                    ProductId = pi.ProductId,
                    IngredientId = pi.IngredientId,
                    IngredientName = pi.Ingredient.Name,
                    QuantityRequired = pi.QuantityRequired
                })
                .FirstOrDefaultAsync();
        }
    }
}
