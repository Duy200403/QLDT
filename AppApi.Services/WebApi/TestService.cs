using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.EmailConfig;
using AppApi.DTO.Models.Test;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Services.WebApi
{
  public interface ITestService : IBaseService<Test>
  {
    Task<PagedResult<TestResponse>> GetAllPaging(TestPagingFilter request);
  }
  
  public class TestService : BaseService<Test>, ITestService
  {

    private readonly IMapper _mapper;
    public TestService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
      _mapper = mapper;
    }

    public async Task<PagedResult<TestResponse>> GetAllPaging(TestPagingFilter request)
    {
      var predicateFilter = PredicateBuilder.True<Test>(); // khởi tạo mệnh đề truy vấn linq
      predicateFilter = predicateFilter.And(x => true);

      if (!string.IsNullOrEmpty(request.Keyword))
      {
        string key = request.Keyword.ToLower().Trim();
        predicateFilter = predicateFilter.And(x => x.TestName.ToLower().Contains(key) || x.TestCode.ToLower().Contains(key)); // thêm điều kiện truy vấn
      }
      // Paging
      long totalRow = await _unitOfWork.Test.CountRecordAsync(predicateFilter);

      var data = await _unitOfWork.Test.ListPaging(predicateFilter, null, null, (request.PageIndex - 1) * request.PageSize, request.PageSize);

      var pagedResult = new PagedResult<TestResponse>()
      {
        TotalRecords = totalRow,
        PageSize = request.PageSize,
        PageIndex = request.PageIndex,
        Data = _mapper.Map<IEnumerable<TestResponse>>(data)
      };

      return pagedResult;
    }

    public override async Task<Test> UpsertAsync(Test entity)
    {
      var result = await _unitOfWork.Test.UpsertAsync(entity);
      await _unitOfWork.CompleteAsync();
      return result;
    }
  }
}