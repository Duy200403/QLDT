using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.GoiThauKeHoach;
using AppApi.Entities.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Services.WebApi
{
    public interface IGoiThauKeHoachService : IBaseService<GoiThauKeHoach>
    {
        Task<PagedResult<GoiThauKeHoachResponse>> GetAllPaging(GoiThauKeHoachFilter request);
    }

    public class GoiThauKeHoachService : BaseService<GoiThauKeHoach>, IGoiThauKeHoachService
    {
        private readonly IMapper _mapper;

        public GoiThauKeHoachService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<PagedResult<GoiThauKeHoachResponse>> GetAllPaging(GoiThauKeHoachFilter request)
        {
            var predicate = PredicateBuilder.True<GoiThauKeHoach>();
            predicate = predicate.And(x => true);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var key = request.Keyword.Trim().ToLower();
                predicate = predicate.And(x =>
                    (x.MaGoi ?? "").ToLower().Contains(key)
                    || x.TenGoiThau.ToLower().Contains(key));
            }

            if (request.NamKeHoach.HasValue)
                predicate = predicate.And(x => x.NamKeHoach == request.NamKeHoach.Value);

            if (!string.IsNullOrWhiteSpace(request.TrangThaiKeHoach))
                predicate = predicate.And(x => x.TrangThaiKeHoach == request.TrangThaiKeHoach);

            if (request.DonViDeXuatChinhId.HasValue)
                predicate = predicate.And(x => x.DonViDeXuatChinhId == request.DonViDeXuatChinhId.Value);

            if (request.DonViMuaSamId.HasValue)
                predicate = predicate.And(x => x.DonViMuaSamId == request.DonViMuaSamId.Value);

            long total = await _unitOfWork.GoiThauKeHoach.CountRecordAsync(predicate);

            var data = await _unitOfWork.GoiThauKeHoach.ListPaging(
                predicate, null, null,
                (request.PageIndex - 1) * request.PageSize,
                request.PageSize
            );

            return new PagedResult<GoiThauKeHoachResponse>
            {
                TotalRecords = total,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = _mapper.Map<IEnumerable<GoiThauKeHoachResponse>>(data)
            };
        }

        public override async Task<GoiThauKeHoach> UpsertAsync(GoiThauKeHoach entity)
        {
            var result = await _unitOfWork.GoiThauKeHoach.UpsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return result;
        }
    }
}
