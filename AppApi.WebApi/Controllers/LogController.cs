using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppApi.DTO.Common;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.AuthService;
using AppApi.Services.LogServ;
using AppApi.DTO.Models.Logger;
using Microsoft.AspNetCore.Authorization;

namespace AppApi.WebApi.Controllers
{
    public class LogController : BaseController
    {
        private readonly ILogger<LogController> _logger;
        private readonly ILogService _logService;

        public LogController(ILogger<LogController> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] LogPagingFilter request)
        {
            var pagedResult = await _logService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _logService.DeleteAsync(id);
            return result ? Ok(id) : BadRequest(id);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteByDate([FromBody] LogRequest request)
        {
            var listLog = await _logService.GetAllByDate(request);
            _logService.LogDeleteMany(listLog);

            return Ok(request);
        }
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> CountLogsByDate([FromQuery] RequestFilterDateBase request)
        {
            var allCount = await _logService.CountLogsByDate(request);
            return Ok(allCount);
        }
    }
}