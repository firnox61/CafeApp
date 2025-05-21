using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITableService
    {
        Task<IDataResult<List<TableGetDto>>> GetAllAsync();
        Task<IDataResult<TableGetDto?>> GetById(int id);
        Task<IResult> Add(TableCreateDto tableCreateDto);
        Task<IResult> Update(TableUpdateDto tableUpdateDto);
        Task<IResult> Delete(Table table);
    }
}
