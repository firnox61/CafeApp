using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("add")]
        [Consumes("multipart/form-data")] // 🔥 Swagger ve ASP.NET'e sinyal ver
        public async Task<IActionResult> Add([FromForm] ProductCreateDto productCreateDto)
        {
            try
            {
                var result = await _productService.Add(productCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 EXCEPTION: " + ex.Message);
                Console.WriteLine("🔥 STACKTRACE: " + ex.StackTrace);
                return StatusCode(500, $"Beklenmeyen hata: {ex.Message}");
            }
        }


        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateDto productUpdateDto)
        {
            var result = await _productService.Update(productUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _productService.Delete(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("getmostproducedproduct")]
        public async Task<IActionResult> GetMostProducedProducts()
        {
            var result = await _productService.GetMostProducedProductsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("production-history")]
        public async Task<IActionResult> GetProductionHistory()
        {
            var result = await _productService.GetProductionHistoryReportAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }


        




    }
}
