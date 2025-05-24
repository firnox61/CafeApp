using AutoMapper;
using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class IngredientManager : IIngredientService
    {
        private readonly IIngredientDal _ingredientDal;
        private readonly IMapper _mapper;

        public IngredientManager(IIngredientDal ıngredientDal, IMapper mapper)
        {
            _ingredientDal = ıngredientDal;
            _mapper = mapper;
        }
        // [CacheRemoveAspect("ITableService.Get")]
        //  [ValidationAspect(typeof(IngredientCreateDtoValidator))]
        //    [LogAspect(typeof(FileLogger))] // opsiyonel: loglama da ekli
      //  [CacheRemoveAspect("CafeApp.Business.Concrete.IngredientManager.GetAllAsync")]
        public async Task<IResult> Add(IngredientCreateDto ingredientCreateDto)
        {
            var newIngredient= _mapper.Map<Ingredient>(ingredientCreateDto);
            await _ingredientDal.AddAsync(newIngredient);
            return new SuccessResult();
        }

        public async Task<IResult> Delete(int ingredientId)
        {
            var ingredient = await _ingredientDal.GetAsync(p => p.Id == ingredientId);
            if (ingredient == null)
                return new ErrorResult("Malzeme bulunamadı.");

            await _ingredientDal.DeleteAsync(ingredient);
            return new SuccessResult("Malzeme silindi.");
        }
      //  [CacheAspect(duration: 10)]
        public async Task<IDataResult<List<IngredientDto>>> GetAllAsync()
        {
            var ingredientEntities = await _ingredientDal.GetAllAsync();
            var dtoList = _mapper.Map<List<IngredientDto>>(ingredientEntities);

            Console.WriteLine("IngredientDto listesi başarıyla maplendi");

            return new SuccessDataResult<List<IngredientDto>>(dtoList);

        }

        public async Task<IDataResult<Ingredient?>> GetById(int id)
        {
            
            return new SuccessDataResult<Ingredient?>(await _ingredientDal.GetAsync(i => i.Id == id));
        }

        public async Task<IResult> Update(IngredientUpdateDto ingredientUpdateDto)
        {
            var Ingredient = _mapper.Map<Ingredient>(ingredientUpdateDto);
            await _ingredientDal.UpdateAsync(Ingredient);
            return new SuccessResult(); 
        }
        public async Task<IDataResult<List<StockAlertDto>>> GetCriticalStockIngredientsAsync()
        {
            var allIngredients = await _ingredientDal.GetAllAsync(i => i.Stock <= i.MinStockThreshold);
            var dtoList = _mapper.Map<List<StockAlertDto>>(allIngredients);
            return new SuccessDataResult<List<StockAlertDto>>(dtoList);
        }
    }
}
