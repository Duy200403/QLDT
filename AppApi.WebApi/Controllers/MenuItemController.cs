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
using AppApi.DTO.Models.MenuItemDto;
using Microsoft.AspNetCore.Authorization;
using AppApi.DataAccess.Base;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AppApi.WebApi.Controllers
{
    public class MenuItemController : BaseController
    {
        private readonly ILogger<MenuItemController> _logger;
        private readonly IMenuItemService _menuItemService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public MenuItemController(ApplicationDbContext dbContext, ILogger<MenuItemController> logger, IMapper mapper, IMenuItemService menuItemService, ILogService logService)
        {
            _logger = logger;
            _menuItemService = menuItemService;
            _logService = logService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(MenuItemRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<MenuItem>(model);
            // Load các ApiRoleMapping tương ứng
            if (model.ApiRoleMappings?.Any() == true)
            {
                var lstApiRoleMappingId = model.ApiRoleMappings.Select(x => x.Id).ToList();
                item.ApiRoleMappings = await _dbContext.ApiRoleMapping
                    .Where(m => lstApiRoleMappingId.Contains(m.Id))
                    .ToListAsync();
            }
            item.Id = Guid.NewGuid();
            item.CreatedBy = username;
            var result = await _menuItemService.AddOneAsync(item);

            var response = _mapper.Map<MenuItemResponse>(item);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "MenuItemController, Create, " + (result ? "OK" : "not OK"), paramTrace);

            return Ok(response);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(_mapper.Map<MenuItemResponse>(item));
        }


        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] MenuItemPagingFilter request)
        {
            var pagedResult = await _menuItemService.GetAllPaging(request);
            return Ok(pagedResult);
        }

        // [Authorize(Policy = "DynamicRoles")]
        // [HttpGet("[action]")]
        // public async Task<IActionResult> GetMenu()
        // {
        //     var userRoles = User.Claims
        //         .Where(c => c.Type == "role")
        //         .Select(c => c.Value)
        //         .ToList();

        //     // Lấy menu + mapping trong một truy vấn
        //     var all = await _dbContext.MenuItem
        //       .Include(m => m.ApiRoleMappings)
        //       .Include(m => m.Children)
        //       .AsNoTracking()
        //       .ToListAsync();

        //     // Đệ quy build tree, filter mapping per-menu:
        //     IEnumerable<MenuItemResponse> BuildTree(Guid? parentId)
        //     {
        //         return all
        //           .Where(m => m.ParentId == parentId)
        //           .Select(m => new
        //           {
        //               Entity = m,
        //               Children = BuildTree(m.Id).ToList()
        //           })
        //           // Giữ lại nếu bản thân có mapping phù hợp
        //           // hoặc bất kỳ con nào có mapping phù hợp
        //           .Where(x =>
        //               // 1) Parent có quyền
        //               x.Entity.ApiRoleMappings
        //                 .Any(r => r.LstAllowedRoles
        //                             .Intersect(userRoles, StringComparer.OrdinalIgnoreCase)
        //                             .Any())
        //               ||
        //               // 2) Hoặc có ít nhất một child
        //               x.Children.Count > 0
        //           )
        //           // Chuyển về DTO
        //           .Select(x => new MenuItemResponse
        //           {
        //               Id = x.Entity.Id,
        //               Title = x.Entity.Title,
        //               Icon = x.Entity.Icon,
        //               Path = x.Entity.Path,
        //               Children = x.Children
        //           })
        //           .ToList();
        //     }
        //     return Ok(BuildTree(null));
        // }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, MenuItemRequest model)
        {
            var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var item = _mapper.Map<MenuItem>(model);
            item.Id = id;
            item.UpdatedBy = username;

            var itemUpdated = await _menuItemService.UpsertAsync(item);

            var response = _mapper.Map<MenuItemResponse>(itemUpdated);

            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "MenuItemController, UpdateItem, Ok", paramTrace);
            return Ok(response);
        }

        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("UpdateTree")]
        public async Task<IActionResult> UpdateTree(
        [FromBody] List<MenuItemTreeUpdateRequest> tree)
        {
            // 1) Flatten the nested DTO into (Id, ParentId) pairs
            var flat = new List<(Guid Id, Guid? ParentId)>();
            void Flatten(IEnumerable<MenuItemTreeUpdateRequest> nodes, Guid? parentId)
            {
                foreach (var node in nodes)
                {
                    flat.Add((node.Id, parentId));
                    if (node.Children?.Any() == true)
                        Flatten(node.Children, node.Id);
                }
            }
            Flatten(tree, null);

            // 2) Batch-fetch all affected MenuItems
            var ids = flat.Select(x => x.Id).ToList();
            var items = await _dbContext.MenuItem
                                 .Where(m => ids.Contains(m.Id))
                                 .ToListAsync();

            // 3) Apply the new ParentId to each entity
            foreach (var (id, parentId) in flat)
            {
                var menuItem = items.FirstOrDefault(m => m.Id == id);
                if (menuItem != null)
                    menuItem.ParentId = parentId;
            }

            // 4) Persist in one go
            await _dbContext.SaveChangesAsync();

            return Ok();  // or return NoContent()
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _menuItemService.DeleteAsync(id);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "MenuItemController, DeleteItem, " + (result ? "OK" : "not OK"), id.ToString());
            return result ? Ok(id) : BadRequest(id);
        }
    }
}