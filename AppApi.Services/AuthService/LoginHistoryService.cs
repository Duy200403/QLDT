using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.Entities.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.Entities.Models.Base;
using AppApi.DTO.Models.LoginHistory;

namespace AppApi.Services.AuthService
{
    public interface ILoginHistoryService : IBaseService<LoginHistory>
    {
        Task<PagedResult<LoginHistoryResponse>> GetAllPaging(LoginHistoryPagingFilter request);
        Task<dynamic> Create(string userName, Guid id);
        Task<IEnumerable<dynamic>> GetStatisticsLoginByAccount(LoginHisReportRequest request);
        Task<dynamic> CountAll();
        Task<dynamic> CountAllByDate(RequestFilterDateBase request);
    }
    public class LoginHistoryService : BaseService<LoginHistory>, ILoginHistoryService
    {
        private readonly AuthDbContext _dbContext;
        private readonly IMapper _mapper;
        public LoginHistoryService(IUnitOfWork unitOfWork, AuthDbContext dbContext, IMapper mapper) : base(unitOfWork)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // public async Task<dynamic> CountAll()
        // {
        //     var predicateFilter = PredicateBuilder.True<LoginHistory>().And(x => true);
        //     long totalRow = await _unitOfWork.LoginHistory.CountRecordAsync(predicateFilter);
        //     return new
        //     {
        //         total = totalRow
        //     };
        // }

        // public async Task<dynamic> CountAllByDate(RequestFilterDateBase request)
        // {
        //     // var predicateFilter = PredicateBuilder.True<LoginHistory>().And(x => true);
        //     // if (!string.IsNullOrEmpty(request.fromDate.ToString()))
        //     // {
        //     //     var fromDate = request.fromDate.Value;
        //     //     fromDate = fromDate.AddDays(-1);
        //     //     predicateFilter = predicateFilter.And(x => x.LoginDate.Date > fromDate.Date);
        //     // }
        //     // if (!string.IsNullOrEmpty(request.toDate.ToString()))
        //     // {
        //     //     var toDate = request.toDate.Value;
        //     //     toDate = toDate.AddDays(1);
        //     //     predicateFilter = predicateFilter.And(x => x.LoginDate.Date < toDate.Date);
        //     // }
        //     // long totalRow = await _unitOfWork.LoginHistory.CountRecordAsync(predicateFilter);
        //     // return new
        //     // {
        //     //     total = totalRow
        //     // };

        //     // var fromDate = request.fromDate.Value;
        //     // fromDate = fromDate.AddDays(-1);
        //     // var toDate = request.toDate.Value;
        //     var yesterday = DateTime.Now.AddDays(-1);
        //     // toDate = toDate.AddDays(1);

        //     var fromDate = Util.FloorDate(request.fromDate);
        //     var toDate = Util.CeilDate(request.toDate);

        //     var countByDate =
        //                new
        //                {
        //                    Stt = 1,
        //                    Title = "count",
        //                    GetCountToday = (from x in _dbContext.LoginHistory
        //                                  where x.LoginDate.Date < toDate.Value.Date
        //                                  where x.LoginDate.Date > yesterday.Date
        //                                  select x).Count(),
        //                    GetCountSevenDay = (from x in _dbContext.LoginHistory
        //                                     where x.LoginDate.Date < toDate.Value.Date
        //                                     where x.LoginDate.Date > fromDate.Value.Date
        //                                     select x).Count()
        //                };

        //     return countByDate;
        // }

        public async Task<dynamic> Create(string userName, Guid id)
        {
            LoginHistory loginHistory = new LoginHistory
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                AccountId = id,
                LoginDate = DateTime.UtcNow,
            };
            await this.AddOneAsync(loginHistory);
            return null;
        }

        public async Task<PagedResult<LoginHistoryResponse>> GetAllPaging(LoginHistoryPagingFilter request)
        {
            var predicateFilter = PredicateBuilder.True<LoginHistory>();
            predicateFilter = predicateFilter.And(x => true);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                string key = request.Keyword.ToLower().Trim();
                predicateFilter = predicateFilter.And(x => x.UserName.ToLower().Contains(key));
            }
            long totalRow = await this.CountAsync(predicateFilter);

            var resultData = await this.GetSortedPaginatedAsync(predicateFilter, nameof(LoginHistory.LoginDate), SortDirection.DESC, request.PageIndex, request.PageSize);

            var pagedResult = new PagedResult<LoginHistoryResponse>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = _mapper.Map<IEnumerable<LoginHistoryResponse>>(resultData)
            };
            return pagedResult;

        }
        // Thống kê đăng nhập theo tài khoản
        public async Task<IEnumerable<dynamic>> GetStatisticsLoginByAccount(LoginHisReportRequest request)
        {
            var fromDate = Util.FloorDate(request.fromDate);
            var toDate = Util.CeilDate(request.toDate);

            var countQuery = from p in _dbContext.LoginHistory
                             where request.fromDate != null ? p.LoginDate.Date > fromDate.Value.Date : 1 == 1
                             where request.toDate != null ? p.LoginDate.Date < toDate.Value.Date : 1 == 1
                             group p by p.UserName into g
                             select new
                             {
                                 UserName = g.Key,
                                 TotalLogin = g.Count()
                             };
            return await countQuery.ToListAsync();
        }
        // Đếm tất cả số lượng đăng nhập
        public async Task<dynamic> CountAll()
        {
            var predicateFilter = PredicateBuilder.True<LoginHistory>().And(x => true);
            long totalRow = await _unitOfWork.LoginHistory.CountRecordAsync(predicateFilter);
            return new
            {
                total = totalRow
            };
        }
        // Thống kê số lượng truy cập theo ngày ( áp dụng lấy theo ngày hôm qua và 7 ngày trước)
        public async Task<dynamic> CountAllByDate(RequestFilterDateBase request)
        {
            // var predicateFilter = PredicateBuilder.True<LoginHistory>().And(x => true);
            // if (!string.IsNullOrEmpty(request.fromDate.ToString()))
            // {
            //     var fromDate = request.fromDate.Value;
            //     fromDate = fromDate.AddDays(-1);
            //     predicateFilter = predicateFilter.And(x => x.LoginDate.Date > fromDate.Date);
            // }
            // if (!string.IsNullOrEmpty(request.toDate.ToString()))
            // {
            //     var toDate = request.toDate.Value;
            //     toDate = toDate.AddDays(1);
            //     predicateFilter = predicateFilter.And(x => x.LoginDate.Date < toDate.Date);
            // }
            // long totalRow = await _unitOfWork.LoginHistory.CountRecordAsync(predicateFilter);
            // return new
            // {
            //     total = totalRow
            // };

            // var fromDate = request.fromDate.Value;
            // fromDate = fromDate.AddDays(-1);
            // var toDate = request.toDate.Value;
            // toDate = toDate.AddDays(1);

            var yesterday = DateTime.Now.AddDays(-1);
            var fromDate = Util.FloorDate(request.fromDate);
            var toDate = Util.CeilDate(request.toDate);

            var countByDate =
                       new
                       {
                           Stt = 1,
                           Title = "count",
                           GetCountToday = (from x in _dbContext.LoginHistory
                                            where x.LoginDate.Date < toDate.Value.Date
                                            where x.LoginDate.Date > yesterday.Date
                                            select x).Count(),
                           GetCountSevenDay = (from x in _dbContext.LoginHistory
                                               where x.LoginDate.Date < toDate.Value.Date
                                               where x.LoginDate.Date > fromDate.Value.Date
                                               select x).Count()
                       };

            return countByDate;
        }
    }
}