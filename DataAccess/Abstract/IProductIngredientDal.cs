using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductIngredientDal : IEntityRepository<ProductIngredient>
    {
        // Task<List<ProductIngredient>> GetAllWithIncludesAsync();
        Task<List<ProductIngredientGetDto>> GetAllWithDetailsAsync();
        Task<ProductIngredientGetDto?> GetByIdWithDetailsAsync(int ingredientId);
        Task<List<ProductIngredient>> GetAllWithProductAndIngredientAsync();
    }
}
