using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Ingredients;
using Entities.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductIngredientService
    {
        Task<IDataResult<List<ProductIngredientGetDto>>> GetAllAsync();
        Task<IDataResult<ProductIngredientGetDto>> GetById(int id);
        Task<IResult> Add(ProductIngredientCreateDto productIngredientCreateDto);
       // Task<IResult> Update(IngredientUpdateDto ingredientUpdateDto);
        Task<IResult> Delete(ProductIngredient productIngredient);
    }
}
