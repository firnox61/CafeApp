using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs.Ingredients;
using Entities.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductIngredientsController : ControllerBase
    {
        private readonly IProductIngredientService _productIngredientService;

        public ProductIngredientsController(IProductIngredientService productIngredientService)
        {
            _productIngredientService = productIngredientService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(ProductIngredientCreateDto productIngredientCreateDto)
        {
            var result = await _productIngredientService.Add(productIngredientCreateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

       /* [HttpPost("update")]
        public async Task<IActionResult> Update(IngredientUpdateDto ingredientUpdateDto)
        {
            var result = await _productIngredientService.Update(ingredientUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }*/

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(ProductIngredient productIngredient)
        {
            var result = await _productIngredientService.Delete(productIngredient);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productIngredientService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productIngredientService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
