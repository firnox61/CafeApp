using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;
        private readonly IMapper _mapper;

        public OperationClaimManager(IOperationClaimDal operationClaimDal, IMapper mapper)
        {
            _operationClaimDal = operationClaimDal;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync()
        {
            var entities = await _operationClaimDal.GetAllAsync();
            var dtoList = _mapper.Map<List<OperationClaimListDto>>(entities);
            return new SuccessDataResult<List<OperationClaimListDto>>(dtoList);
        }

        public async Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id)
        {
            var entity = await _operationClaimDal.GetAsync(o => o.Id == id);
            if (entity == null)
                return new ErrorDataResult<OperationClaimListDto>("Rol bulunamadı");

            var dto = _mapper.Map<OperationClaimListDto>(entity);
            return new SuccessDataResult<OperationClaimListDto>(dto);
        }

        public async Task<IResult> AddAsync(OperationClaimCreateDto dto)
        {
            var entity = _mapper.Map<OperationClaim>(dto);
            await _operationClaimDal.AddAsync(entity);
            return new SuccessResult("Rol eklendi");
        }

        public async Task<IResult> UpdateAsync(OperationClaimUpdateDto dto)
        {
            var entity = await _operationClaimDal.GetAsync(x => x.Id == dto.Id);
            if (entity == null)
                return new ErrorResult("Güncellenecek rol bulunamadı");

            entity.Name = dto.Name;
            await _operationClaimDal.UpdateAsync(entity);
            return new SuccessResult("Rol güncellendi");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _operationClaimDal.GetAsync(x => x.Id == id);
            if (entity == null)
                return new ErrorResult("Silinecek rol bulunamadı");

            await _operationClaimDal.DeleteAsync(entity);
            return new SuccessResult("Rol silindi");
        }
    }

}