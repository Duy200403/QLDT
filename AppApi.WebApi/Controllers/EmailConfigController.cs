using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AppApi.Common.Helper;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.LogServ;
using AppApi.Services.WebApi;
using AppApi.DTO.Models.EmailConfig;
using Microsoft.AspNetCore.Authorization;
using AppApi.DTO.ElasticSearch;

namespace AppApi.WebApi.Controllers
{
    public class EmailConfigController : BaseController
    {
        private readonly ILogger<EmailConfigController> _logger;
        private readonly IEmailConfigService _emailConfigService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ElasticSearch<EmailConfig> _objSearch;

        public EmailConfigController(ElasticSearch<EmailConfig> objSearch, ILogger<EmailConfigController> logger, IMapper mapper, IEmailConfigService emailConfigService, ILogService logService)
        {
            _logger = logger;
            _emailConfigService = emailConfigService;
            _logService = logService;
            _mapper = mapper;
            _objSearch = objSearch;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEmailConfig(EmailConfigRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<EmailConfig>(model);
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;
            var result = await _emailConfigService.AddOneAsync(item);

            // var respElastic = await _objSearch.AddAsync(item, item.Id, true);

            var response = _mapper.Map<EmailConfigResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "EmailConfigsController, Create, " + (result ? "OK" : "not OK"), paramTrace);

            // return Ok(respElastic);
            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            // var item = await _emailConfigService.GetByIdAsync(id);
            // return item == null ? NotFound() : Ok(_mapper.Map<EmailConfigResponse>(item));

            var item = await _objSearch.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] EmailConfigPagingFilter request)
        {
            var pagedResult = await _emailConfigService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPagingElastic([FromQuery] EmailConfigPagingFilter request)
        {
            var pagedResult = await _objSearch.GetResult(request.Keyword ?? "", request.PageIndex, request.PageSize, true, nameof(EmailConfig.CreatedDate));
            return Ok(pagedResult);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, EmailConfigRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<EmailConfig>(model);
            item.Id = id;
            item.UpdatedBy = username;

            var itemUpdated = await _emailConfigService.UpsertAsync(item);

            // var respElastic = await _objSearch.UpsertAsync(item, item.Id, true);

            var response = _mapper.Map<EmailConfigResponse>(itemUpdated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "EmailConfigsController, UpdateItem, Ok", paramTrace);
            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _emailConfigService.DeleteAsync(id);
            // var ok = await _objSearch.DeleteByIdAsync(id, waitForRefresh: true);
            // await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "EmailConfigsController, DeleteItem, " + (result && ok ? "OK" : "not OK"), id.ToString());
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "EmailConfigsController, DeleteItem, " + (result ? "OK" : "not OK"), id.ToString());
            return result ? Ok(id) : BadRequest(id);
        }
    }
}