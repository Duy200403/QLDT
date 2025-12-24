using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.FileManager;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AutoMapper;

namespace AppApi.Services.WebApi
{
  public interface IFileManagerService : IBaseService<FileManager>
  {
    Task<PagedResult<FileManagerResponse>> GetAllPaging(FileManagerPagingFilter request);
  }
  
  public class FileManagerService : BaseService<FileManager>, IFileManagerService
  {
    private readonly IMapper _mapper;
    public FileManagerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
      _mapper = mapper;
    }
    public async Task<PagedResult<FileManagerResponse>> GetAllPaging(FileManagerPagingFilter request)
    {
      var predicateFilter = PredicateBuilder.True<FileManager>();
      predicateFilter = predicateFilter.And(x => true);

      if (!string.IsNullOrEmpty(request.Keyword))
      {
        string key = request.Keyword.ToLower().Trim();
        predicateFilter = predicateFilter.And(x => x.Name.ToLower().Contains(key));
      }

      long totalRow = await this.CountAsync(predicateFilter);

      var resultData = await this.GetSortedPaginatedAsync(predicateFilter, nameof(FileManager.Id), SortDirection.ASC, request.PageIndex, request.PageSize);

      var pagedResult = new PagedResult<FileManagerResponse>()
      {
        TotalRecords = totalRow,
        PageSize = request.PageSize,
        PageIndex = request.PageIndex,
        Data = _mapper.Map<IEnumerable<FileManagerResponse>>(resultData)
      };
      return pagedResult;
    }
  }
}