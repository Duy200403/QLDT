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
using AppApi.DTO.Models.Test;

namespace AppApi.WebApi.Controllers
{
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly ITestService _testService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public TestController(ILogger<TestController> logger, IMapper mapper, ITestService testService, ILogService logService)
        {
            _logger = logger;
            _testService = testService;
            _logService = logService;
            _mapper = mapper;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(TestRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<Test>(model);
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;
            var result = await _testService.AddOneAsync(item);

            // var respElastic = await _objSearch.AddAsync(item, item.Id, true);

            var response = _mapper.Map<TestResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "TestController, Create, " + (result ? "OK" : "not OK"), paramTrace);

            // return Ok(respElastic);
            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _testService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(_mapper.Map<TestResponse>(item));

            // var item = await _objSearch.GetByIdAsync(id);
            // return item == null ? NotFound() : Ok(item);
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] TestPagingFilter request)
        {
            var pagedResult = await _testService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, TestRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<Test>(model);
            item.Id = id;
            item.UpdatedBy = username;

            var itemUpdated = await _testService.UpsertAsync(item);

            // var respElastic = await _objSearch.UpsertAsync(item, item.Id, true);

            var response = _mapper.Map<TestResponse>(itemUpdated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "TestController, UpdateItem, Ok", paramTrace);
            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _testService.DeleteAsync(id);
            // var ok = await _objSearch.DeleteByIdAsync(id, waitForRefresh: true);
            // await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "EmailConfigsController, DeleteItem, " + (result && ok ? "OK" : "not OK"), id.ToString());
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "TestController, DeleteItem, " + (result ? "OK" : "not OK"), id.ToString());
            return result ? Ok(id) : BadRequest(id);
        }
    }
}