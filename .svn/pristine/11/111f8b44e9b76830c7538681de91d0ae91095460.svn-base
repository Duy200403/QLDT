using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AppApi.Common.Helper;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.LogServ;
using AppApi.Services.AuthService;
using AppApi.DTO.Models.RoleDto;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using AppApi.DTO.Common;
using AppApi.DTO.Models.Response;

namespace AppApi.AuthService.Controllers
{
    public class RoleController : BaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public RoleController(ILogger<RoleController> logger, IMapper mapper, IRoleService roleService, ILogService logService)
        {
            _logger = logger;
            _roleService = roleService;
            _logService = logService;
            _mapper = mapper;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(RoleRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            model.ParentId = Guid.Empty.ToString();

            var item = _mapper.Map<Role>(model);
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;
            var result = await _roleService.AddOneAsync(item);

            var response = _mapper.Map<RoleResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "RoleController, Create, " + (result ? "OK" : "not OK"), paramTrace);

            return Ok(response);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PagedResult<RoleResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var listCate = await _roleService.AllAsync();
            var pagedResult = new PagedResult<RoleResponse>()
            {
                Data = _mapper.Map<IEnumerable<RoleResponse>>(listCate)
            };
            return Ok(pagedResult);
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _roleService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(_mapper.Map<RoleResponse>(item));
        }

        // [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoleAdmin()
        {
            var item = await _roleService.GetOneAsync(x => x.Name == RoleEnum.admin.ToString());
            return item == null ? NotFound() : Ok(_mapper.Map<RoleResponse>(item));
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] RolePagingFilter request)
        {
            var pagedResult = await _roleService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, RoleRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var oldItem = await _roleService.GetByIdAsync(id);

            if (oldItem == null)
            {
                ErrorResponseModel error = new ErrorResponseModel
                {
                    Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.idNotFound } } }
                };
                return BadRequest(error);
            }

            model.ParentId = Guid.Empty.ToString();
            var item = _mapper.Map<Role>(model);
            item.Id = id;
            item.ParentId = oldItem.ParentId;
            item.UpdatedBy = username;

            var itemUpdated = await _roleService.UpsertAsync(item);

            var response = _mapper.Map<RoleResponse>(itemUpdated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "RoleController, UpdateItem, Ok", paramTrace);
            return Ok(response);
        }

        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateTree(List<RoleTreeRequest> tree)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            if (tree.Any())
            {
                // sắp xếp lại cây
                int index = 1;
                foreach (var node in tree)
                {
                    await UpdateTreeNode(Guid.Empty, node, index, username);
                    ++index;
                }
            }
            return Ok(new { success = true });
        }

        private async Task UpdateTreeNode(Guid parentId, RoleTreeRequest rootNode, int index, string username)
        {
            // update rootNode
            var role = _mapper.Map<Role>(rootNode);
            role.ParentId = parentId;
            role.OrderNumber = index;
            role.UpdatedBy = username;
            var result = await _roleService.UpdateAsync(role);

            // update children của rootNode
            if (result && rootNode.Children.Any())
            {
                int recursiveIndex = 1;
                foreach (var node in rootNode.Children)
                {
                    await UpdateTreeNode(role.Id, node, recursiveIndex, username);
                    ++recursiveIndex;
                }
            }
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _roleService.DeleteAsync(id);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "RoleController, DeleteItem, " + (result ? "OK" : "not OK"), id.ToString());
            return result ? Ok(id) : BadRequest(id);
        }
    }
}