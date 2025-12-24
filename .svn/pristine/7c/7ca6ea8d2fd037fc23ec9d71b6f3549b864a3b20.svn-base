using System.Net;
using System.Threading.Tasks;
using AppApi.DTO.Common;
using AppApi.DTO.Models.LoginHistory;
using AppApi.Entities.Models.Base;
using AppApi.Services.AuthService;
using AppApi.Services.LogServ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.AuthService.Controllers
{
  public class LoginHistoryController : BaseController
  {
    private readonly ILogger<LoginHistoryController> _logger;
    private readonly ILoginHistoryService _iLoginHistoryService;
    private readonly ILogService _logService;
    public LoginHistoryController(ILogger<LoginHistoryController> logger, ILoginHistoryService iLoginHistoryService, ILogService logService)
    {
      _logger = logger;
      _iLoginHistoryService = iLoginHistoryService;
      _logService = logService;
    }
    // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
    [Authorize(Policy = "DynamicRoles")]
    [HttpGet("[action]")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(PagedResult<LoginHistoryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllPaging([FromQuery] LoginHistoryPagingFilter request)
    {
      var result = await _iLoginHistoryService.GetAllPaging(request);
      return Ok(result);
    }
    // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
    [Authorize(Policy = "DynamicRoles")]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetStatisticsLoginByAccount([FromQuery] LoginHisReportRequest request)
    {
      var listCount = await _iLoginHistoryService.GetStatisticsLoginByAccount(request);
      return Ok(listCount);
    }
    // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
    [Authorize(Policy = "DynamicRoles")]
    [HttpGet("[action]")]
    public async Task<IActionResult> CountAllLogin()
    {
      var allCount = await _iLoginHistoryService.CountAll();
      return Ok(allCount);
    }
    // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
    [Authorize(Policy = "DynamicRoles")]
    [HttpGet("[action]")]
    public async Task<IActionResult> CountAllByDate([FromQuery] RequestFilterDateBase request)
    {
      var allCount = await _iLoginHistoryService.CountAllByDate(request);
      return Ok(allCount);
    }
  }
}