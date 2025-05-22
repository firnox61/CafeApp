using Business.Abstract;
using Business.Utilites;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IProductIngredientService _productIngredientService;

        public ReportsController(IProductIngredientService productIngredientService)
        {
            _productIngredientService = productIngredientService;
        }

        [HttpPost("exportmostusedingredients-excel")]
        public async Task<IActionResult> ExportMostUsedIngredientsExcel([FromBody] UsageReportFilterDto filter)
        {
            var result = await _productIngredientService.GetMostUsedIngredientsAsync(filter);

            if (!result.Success || result.Data == null || !result.Data.Any())
                return BadRequest("Veri bulunamadı veya boş.");

            var excelBytes = ExcelExportHelper.ExportIngredientUsageToExcel(result.Data);

            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"EnCokKullanilanMalzemeler_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

    }
}
