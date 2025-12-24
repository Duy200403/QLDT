using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.ApiRoleMapping;
using AppApi.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Services.Common
{
    public interface IApiRoleMappingService : IBaseService<ApiRoleMapping>
    {
        Task<PagedResult<ApiRoleMappingResponse>> GetAllPaging(ApiRoleMappingPagingFilter request);
    }

    public class ApiRoleMappingService : BaseService<ApiRoleMapping>, IApiRoleMappingService
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public ApiRoleMappingService(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagedResult<ApiRoleMappingResponse>> GetAllPaging(ApiRoleMappingPagingFilter request)
        {
            var predicateFilter = PredicateBuilder.True<ApiRoleMapping>(); // khởi tạo mệnh đề truy vấn linq
            predicateFilter = predicateFilter.And(x => true);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                string key = request.Keyword.ToLower().Trim();
                predicateFilter = predicateFilter.And(x => x.Controller.ToLower().Contains(key)); // thêm điều kiện truy vấn
            }
            // Paging
            long totalRow = await _unitOfWork.ApiRoleMapping.CountRecordAsync(predicateFilter);

            var data = await _unitOfWork.ApiRoleMapping.ListPaging(predicateFilter, null, null, (request.PageIndex - 1) * request.PageSize, request.PageSize);

            var pagedResult = new PagedResult<ApiRoleMappingResponse>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = _mapper.Map<IEnumerable<ApiRoleMappingResponse>>(data)
            };

            return pagedResult;
        }

        public override async Task<ApiRoleMapping> UpsertAsync(ApiRoleMapping entity)
        {
            var result = await _unitOfWork.ApiRoleMapping.UpsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return result;
        }
    }
}