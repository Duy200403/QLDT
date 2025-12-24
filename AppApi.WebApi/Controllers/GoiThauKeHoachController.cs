using AppApi.Common.Helper;
using AppApi.DTO.Models.GoiThauKeHoach;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.LogServ;
using AppApi.Services.WebApi;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.WebApi.Controllers
{
    public class GoiThauKeHoachController : BaseController
    {
        private readonly ILogger<GoiThauKeHoachController> _logger;
        private readonly IGoiThauKeHoachService _service;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public GoiThauKeHoachController(
            ILogger<GoiThauKeHoachController> logger,
            IMapper mapper,
            IGoiThauKeHoachService service,
            ILogService logService)
        {
            _logger = logger;
            _service = service;
            _logService = logService;
            _mapper = mapper;
        }

        //[Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(GoiThauKeHoachRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
            //if (string.IsNullOrEmpty(username))
            //    return Unauthorized(new { message = "Không xác định được người dùng." });

            var item = _mapper.Map<GoiThauKeHoach>(model);
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;

            // nếu AuditEntity không auto-set thì giữ 2 dòng này để chắc chắn
            item.CreatedDate = DateTime.UtcNow;
            item.UpdatedBy = username;
            item.UpdatedDate = DateTime.UtcNow;

            var result = await _service.AddOneAsync(item);

            var response = _mapper.Map<GoiThauKeHoachResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace,
                "GoiThauKeHoachController, Create, " + (result ? "OK" : "not OK"),
                paramTrace);

            return Ok(response);
        }

        //[Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(_mapper.Map<GoiThauKeHoachResponse>(item));
        }

        //[Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] GoiThauKeHoachFilter request)
        {
            var pagedResult = await _service.GetAllPaging(request);
            return Ok(pagedResult);
        }

        //[Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, GoiThauKeHoachRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Không xác định được người dùng." });

            var item = _mapper.Map<GoiThauKeHoach>(model);
            item.Id = id;
            item.UpdatedBy = username;
            item.UpdatedDate = DateTime.UtcNow;

            var updated = await _service.UpsertAsync(item);

            var response = _mapper.Map<GoiThauKeHoachResponse>(updated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "GoiThauKeHoachController, UpdateItem, Ok", paramTrace);

            return Ok(response);
        }

        //[Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace,
                "GoiThauKeHoachController, DeleteItem, " + (result ? "OK" : "not OK"),
                id.ToString());

            return result ? Ok(id) : BadRequest(id);
        }
    }
}
