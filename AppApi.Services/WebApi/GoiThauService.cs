//using AppApi.Common.Helper;
//using AppApi.DataAccess.Base;
//using AppApi.DTO.Common;
//using AppApi.DTO.Models.GoiThau;
//using AppApi.Entities.Models;
//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppApi.Services.WebApi
//{
//    public interface IGoiThauService : IBaseService<GoiThauDeXuat>
//    {
//        Task<PagedResult<GoiThauResponse>> GetAllPaging(GoiThauFilter request);
//    }

//    public class GoiThauService : BaseService<GoiThauDeXuat>, IGoiThauService
//    {
//        private readonly IMapper _mapper;

//        public GoiThauService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
//        {
//            _mapper = mapper;
//        }

//        public async Task<PagedResult<GoiThauResponse>> GetAllPaging(GoiThauFilter request)
//        {
//            var predicateFilter = PredicateBuilder.True<GoiThauDeXuat>(); // khởi tạo mệnh đề truy vấn linq
//            predicateFilter = predicateFilter.And(x => true);

//            if (!string.IsNullOrEmpty(request.Keyword))
//            {
//                string key = request.Keyword.ToLower().Trim();
//                predicateFilter = predicateFilter.And(x =>
//                  x.MaGoiThau.ToLower().Contains(key) ||
//                  x.TenGoiThau.ToLower().Contains(key)
//                ); // thêm điều kiện truy vấn
//            }

//            if (request.NamKeHoach.HasValue)
//            {
//                int year = request.NamKeHoach.Value;
//                predicateFilter = predicateFilter.And(x => x.NgayKhoiTao.Year == year);
//            }

//            if (request.TrangThaiHoSoId.HasValue)
//            {
//                int statusId = request.TrangThaiHoSoId.Value;
//                predicateFilter = predicateFilter.And(x => x.TrangThaiHoSoId == statusId);
//            }

//            // Paging
//            long totalRow = await _unitOfWork.GoiThau.CountRecordAsync(predicateFilter);

//            var data = await _unitOfWork.GoiThau.ListPaging(
//              predicateFilter,
//              null,
//              null,
//              (request.PageIndex - 1) * request.PageSize,
//              request.PageSize
//            );

//            var pagedResult = new PagedResult<GoiThauResponse>()
//            {
//                TotalRecords = totalRow,
//                PageSize = request.PageSize,
//                PageIndex = request.PageIndex,
//                Data = _mapper.Map<IEnumerable<GoiThauResponse>>(data)
//            };

//            return pagedResult;
//        }

//        public override async Task<GoiThauDeXuat> UpsertAsync(GoiThauDeXuat entity)
//        {
//            var result = await _unitOfWork.GoiThau.UpsertAsync(entity);
//            await _unitOfWork.CompleteAsync();
//            return result;
//        }
//    }
//}
