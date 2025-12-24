using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AppApi.DTO.Models.MenuItemDto;

namespace AppApi.Services.Common
{
    public interface IMenuItemService : IBaseService<MenuItem>
    {
        Task<PagedResult<MenuItemResponse>> GetAllPaging(MenuItemPagingFilter request);
    }

    public class MenuItemService : BaseService<MenuItem>, IMenuItemService
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public MenuItemService(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagedResult<MenuItemResponse>> GetAllPaging(MenuItemPagingFilter request)
        {
            var predicateFilter = PredicateBuilder.True<MenuItem>(); // khởi tạo mệnh đề truy vấn linq
            predicateFilter = predicateFilter.And(x => true);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                string key = request.Keyword.ToLower().Trim();
                predicateFilter = predicateFilter.And(x => x.Title.ToLower().Contains(key)); // thêm điều kiện truy vấn
            }
            // Paging
            long totalRow = await _unitOfWork.MenuItem.CountRecordAsync(predicateFilter);

            var data = await _unitOfWork.MenuItem.ListPaging(predicateFilter, null, null, (request.PageIndex - 1) * request.PageSize, request.PageSize);

            var pagedResult = new PagedResult<MenuItemResponse>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = _mapper.Map<IEnumerable<MenuItemResponse>>(data)
            };

            return pagedResult;
        }

        public override async Task<MenuItem> UpsertAsync(MenuItem entity)
        {
            var result = await _unitOfWork.MenuItem.UpsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return result;
        }
    }
}