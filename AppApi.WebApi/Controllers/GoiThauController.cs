//using AppApi.Common.Helper;
//using AppApi.DTO.Common;
//using AppApi.DTO.Models.GoiThau;
//using AppApi.Entities.Models;
//using AppApi.Entities.Models.Base;
//using AppApi.Services.LogServ;
//using AppApi.Services.WebApi;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;

//namespace AppApi.WebApi.Controllers
//{
//    public class GoiThauController : BaseController
//    {
//        private readonly ILogger<GoiThauController> _logger;
//        private readonly IGoiThauService _goiThauService;
//        private readonly ILogService _logService;
//        private readonly IMapper _mapper;

//        public GoiThauController(
//            ILogger<GoiThauController> logger,
//            IGoiThauService goiThauService,
//            ILogService logService,
//            IMapper mapper)
//        {
//            _logger = logger;
//            _goiThauService = goiThauService;
//            _logService = logService;
//            _mapper = mapper;
//        }

//        [HttpPost("[action]")]
//        //[ProducesResponseType(typeof(GoiThauResponse), StatusCodes.Status200OK)]
//        public async Task<IActionResult> Create([FromBody] GoiThauRequest model)
//        {
//            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
//            if (string.IsNullOrEmpty(username))
//            {
//                return Unauthorized("Không xác định được người dùng.");
//            }

//            var entity = _mapper.Map<GoiThauDeXuat>(model);
//            entity.Id = Guid.NewGuid();
//            entity.CreatedBy = username;
//            entity.CreatedDate = DateTime.Now;
//            entity.UpdatedBy = username;
//            entity.UpdatedDate = DateTime.Now;
//            entity.IsDeleted = false;

//            var result = await _goiThauService.AddOneAsync(entity);
//            var response = _mapper.Map<GoiThauResponse>(entity);

//            var paramTrace = JsonConvert.SerializeObject(response);
//            await _logService.AddLogWebInfo(LogLevelWebInfo.trace,
//                "GoiThauController, Create, " + (result ? "OK" : "not OK"),
//                paramTrace);

//            return Ok(response);
//        }

//        [HttpGet("[action]/{id}")]
//        //[ProducesResponseType(typeof(GoiThauResponse), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetItem(Guid id)
//        {
//            var entity = await _goiThauService.GetByIdAsync(id);
//            if (entity == null)
//            {
//                return NotFound();
//            }

//            var response = _mapper.Map<GoiThauResponse>(entity);
//            return Ok(response);
//        }

//        [HttpGet("[action]")]
//        //[ProducesResponseType(typeof(PagedResult<GoiThauResponse>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllByPaging([FromQuery] GoiThauFilter request)
//        {
//            var result = await _goiThauService.GetAllPaging(request);
//            return Ok(result);
//        }

//        [HttpPut("[action]/{id}")]
//        //[ProducesResponseType(typeof(GoiThauResponse), StatusCodes.Status200OK)]
//        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] GoiThauRequest model)
//        {
//            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
//            if (string.IsNullOrEmpty(username))
//            {
//                return Unauthorized("Không xác định được người dùng.");
//            }

//            var entity = _mapper.Map<GoiThauDeXuat>(model);
//            entity.Id = id;
//            entity.UpdatedBy = username;
//            entity.UpdatedDate = DateTime.Now;

//            var updated = await _goiThauService.UpsertAsync(entity);
//            var response = _mapper.Map<GoiThauResponse>(updated);

//            var paramTrace = JsonConvert.SerializeObject(response);
//            await _logService.AddLogWebInfo(LogLevelWebInfo.trace,
//                "GoiThauController, UpdateItem, OK",
//                paramTrace);

//            return Ok(response);
//        }

//        [HttpDelete("[action]/{id}")]
//        //[ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> DeleteItem(Guid id)
//        {
//            var result = await _goiThauService.DeleteAsync(id);

//            var paramTrace = JsonConvert.SerializeObject(new { Id = id });
//            await _logService.AddLogWebInfo(LogLevelWebInfo.trace,
//                "GoiThauController, DeleteItem, " + (result ? "OK" : "not OK"),
//                paramTrace);

//            return Ok(new { Success = result });
//        }
//    }
//}

