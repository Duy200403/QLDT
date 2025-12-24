using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.RoleDto;
using AppApi.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Services.AuthService
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<PagedResult<RoleResponse>> GetAllPaging(RolePagingFilter request);
        Task<bool> UpdateAsync(Role entity);
    }

    public class RoleService : BaseService<Role>, IRoleService
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public RoleService(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagedResult<RoleResponse>> GetAllPaging(RolePagingFilter request)
        {
            var predicateFilter = PredicateBuilder.True<Role>(); // khởi tạo mệnh đề truy vấn linq
            predicateFilter = predicateFilter.And(x => true);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                string key = request.Keyword.ToLower().Trim();
                predicateFilter = predicateFilter.And(x => x.Name.ToLower().Contains(key)); // thêm điều kiện truy vấn
            }
            // Paging
            long totalRow = await _unitOfWork.Role.CountRecordAsync(predicateFilter);

            var data = await _unitOfWork.Role.ListPaging(predicateFilter, null, null, (request.PageIndex - 1) * request.PageSize, request.PageSize);

            var pagedResult = new PagedResult<RoleResponse>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = _mapper.Map<IEnumerable<RoleResponse>>(data)
            };

            return pagedResult;
        }

        public override async Task<Role> UpsertAsync(Role entity)
        {
            var result = await _unitOfWork.Role.UpsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task<bool> UpdateAsync(Role entity)
        {
            try
            {
                var roleItem = await _unitOfWork.Role.GetByIdAsync(entity.Id);

                if (roleItem != null)
                {
                    roleItem.ParentId = entity.ParentId;
                    roleItem.OrderNumber = entity.OrderNumber;
                    roleItem.UpdatedBy = entity.UpdatedBy;
                    roleItem.UpdatedDate = DateTime.UtcNow;

                    roleItem.Name = entity.Name;
                    roleItem.Description = entity.Description;

                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

    }
}