using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        Task<IDataResult<List<ProductGetDto>>> GetAllAsync();
        Task<IDataResult<Product?>> GetById(int id);
        Task<IResult> Add(ProductCreateDto productCreateDto);
        Task<IResult> Update(Product product);
        Task<IResult> Delete(int productId);
        Task<IDataResult<List<ProductProductionReportDto>>> GetMostProducedProductsAsync();
        Task<IDataResult<List<ProductProductionHistoryDto>>> GetProductionHistoryReportAsync();
    }
}
