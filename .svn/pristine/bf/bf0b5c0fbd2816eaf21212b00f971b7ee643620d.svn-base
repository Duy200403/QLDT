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
using AppApi.Services.Common;
using AppApi.DTO.Models.ApiRoleMapping;
using Microsoft.AspNetCore.Authorization;
using AppApi.DataAccess.Base;
using Microsoft.EntityFrameworkCore;

namespace AppApi.WebApi.Controllers
{
    public class ApiRoleMappingController : BaseController
    {
        private readonly ILogger<ApiRoleMappingController> _logger;
        private readonly IApiRoleMappingService _apiRoleMappingService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ApiRoleMappingController(ApplicationDbContext applicationDbContext, ILogger<ApiRoleMappingController> logger, IMapper mapper, IApiRoleMappingService apiRoleMappingService, ILogService logService)
        {
            _logger = logger;
            _apiRoleMappingService = apiRoleMappingService;
            _logService = logService;
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(ApiRoleMappingRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<ApiRoleMapping>(model);
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;
            var result = await _apiRoleMappingService.AddOneAsync(item);

            var response = _mapper.Map<ApiRoleMappingResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "ApiRoleMappingController, Create, " + (result ? "OK" : "not OK"), paramTrace);

            return Ok(response);
        }

        // [Authorize(Policy = "DynamicRoles")]
        // [HttpPost("[action]")]
        // public async Task<IActionResult> BulkAssignRoles([FromBody] List<ApiRoleMappingRequest> models)
        // {
        //     var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
        //     if (string.IsNullOrWhiteSpace(username))
        //         return Unauthorized(new { message = "Không xác định được người dùng." });

        //     if (models is null || models.Count == 0)
        //         return BadRequest(new { message = "Danh sách yêu cầu trống." });

        //     // Lấy các ApiRoleMapping Id hợp lệ client gửi lên
        //     foreach (var model in models)
        //     {
        //         var mappedItem = _mapper.Map<ApiRoleMapping>(model);
        //         var existItem = _applicationDbContext.ApiRoleMapping.Where(x => x.Id == model.Id).Include(x => x.LstAllowedRoles).FirstOrDefault();

        //         var newRoleAddIds = mappedItem.LstAllowedRoles.Select(x => x.Id);
        //         var existRoleList = existItem.LstAllowedRoles;

        //         foreach (var item in existRoleList)
        //         {
        //             var roleNewIds = newRoleAddIds.Where(x => !newRoleAddIds.Contains(item.Id));
        //             existRoleList.AddRange
        //         }
        //     }
        // }

        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpPost("[action]")]
        public async Task<IActionResult> BulkAssignRoles([FromBody] List<ApiRoleMappingRequest> models)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized(new { message = "Không xác định được người dùng." });

            if (models == null || models.Count == 0)
                return BadRequest(new { message = "Danh sách yêu cầu trống." });

            int totalUpdated = 0;
            int totalAdded = 0;

            foreach (var model in models)
            {
                var existItem = await _applicationDbContext.ApiRoleMapping
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (existItem == null) continue;

                // List hiện tại (deserialize JSON)
                var existList = existItem.LstAllowedRoles ?? new List<AllowedRole>();

                // List mới client gửi
                var newList = model.LstAllowedRoles ?? new List<AllowedRole>();

                // So sánh theo Id (chỉ add phần tử chưa có)
                var existIds = existList
                    .Where(x => x != null && x.Id != Guid.Empty)
                    .Select(x => x.Id)
                    .ToHashSet();

                // Lọc role hợp lệ + chưa tồn tại, đồng thời loại trùng lặp trong payload
                var toAdd = newList
                    .Where(x => x != null && x.Id != Guid.Empty && !existIds.Contains(x.Id))
                    .GroupBy(x => x.Id)               // distinct theo Id ngay trong payload
                    .Select(g => g.First())
                    .ToList();

                if (toAdd.Count > 0)
                {
                    existList.AddRange(toAdd);
                    totalAdded += toAdd.Count;
                }

                // Ghi ngược JSON + audit
                existItem.LstAllowedRoles = existList;
                existItem.UpdatedBy = username;
                existItem.UpdatedDate = DateTime.UtcNow;

                totalUpdated++;
            }

            await _applicationDbContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã thêm các role mới (không xóa role cũ).",
                updatedMappings = totalUpdated,
                added = totalAdded
            });
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _apiRoleMappingService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(_mapper.Map<ApiRoleMappingResponse>(item));
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] ApiRoleMappingPagingFilter request)
        {
            var pagedResult = await _apiRoleMappingService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, ApiRoleMappingRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<ApiRoleMapping>(model);
            item.Id = id;
            item.UpdatedBy = username;

            var itemUpdated = await _apiRoleMappingService.UpsertAsync(item);

            var response = _mapper.Map<ApiRoleMappingResponse>(itemUpdated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "ApiRoleMappingController, UpdateItem, Ok", paramTrace);
            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        // [Authorize(Policy = "DynamicRolesManagementSoftware")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _apiRoleMappingService.DeleteAsync(id);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "ApiRoleMappingController, DeleteItem, " + (result ? "OK" : "not OK"), id.ToString());
            return result ? Ok(id) : BadRequest(id);
        }
    }
}