using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IIngredientService
    {
        Task<IDataResult<List<IngredientDto>>> GetAllAsync();
        Task<IDataResult<Ingredient?>> GetById(int id);
        Task<IResult> Add(IngredientCreateDto ingredientCreateDto);
        Task<IResult> Update(IngredientUpdateDto ingredientUpdateDto);
        Task<IResult> Delete(int ingredientId);
        Task<IDataResult<List<StockAlertDto>>> GetCriticalStockIngredientsAsync();
    }
}
