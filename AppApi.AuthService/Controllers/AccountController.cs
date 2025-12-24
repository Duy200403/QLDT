using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.Account;
using AppApi.DTO.Models.Response;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.AuthService;
using AppApi.Services.LogServ;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace AppApi.AuthService.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _iAccountService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly AuthDbContext _dbContext;
        public AccountController(AuthDbContext dbContext, ILogger<AccountController> logger, IAccountService iAccountService, ILogService logService, IMapper mapper)
        {
            _logger = logger;
            _iAccountService = iAccountService;
            _logService = logService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        //[Authorize(Role.admin, Role.doctor, Role.general)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(PagedResult<Account>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPaging([FromQuery] AccountPagingFilter request)
        {
            var result = await _iAccountService.GetAllPaging(request);
            return Ok(result);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateAccountRequest model)
        {
            var result = await _iAccountService.Create(model);
            if (result != null && result.GetType() == typeof(ErrorResponseModel))
            {
                return BadRequest(result);
            }
            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "Tạo tài khoản mới thành công", paramTrace);
            return Ok();
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateAccountRequest model)
        {
            // var authResult = await HttpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            // if (!authResult.Succeeded || authResult.Principal == null || !authResult.Principal.Identity.IsAuthenticated)
            // {
            //     // Nếu xác thực thất bại, trả về 401 + JSON rồi fail
            //     HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //     var error = authResult.Properties?.Items.TryGetValue(OpenIddictValidationAspNetCoreConstants.Properties.Error, out var e) == true ? e : null;
            //     var desc  = authResult.Properties?.Items.TryGetValue(OpenIddictValidationAspNetCoreConstants.Properties.ErrorDescription, out var d) == true ? d : null;

            //     await HttpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
            //     {
            //         message = "Unauthorized",
            //         error,
            //         error_description = desc
            //     }));
            //     return BadRequest(new ErrorResponseModel
            //     {
            //         Errors = new Dictionary<string, string[]>
            //         {
            //             { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new[] { "Unauthorized" } }
            //         }
            //     });
            // }

            var userRoles = HttpContext.User.FindAll(OpenIddictConstants.Claims.Role).Select(r => r.Value).ToList();
            var accountId = HttpContext.User.FindFirstValue(OpenIddictConstants.Claims.Subject);

            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }
            // var account = (Account)HttpContext.Items[ConstantsInternal.Account];
            // Nếu không phải admin và manager thì check có phải nó sửa cho chính nó ko, nếu sửa cho tài khoản khác thì ko được phép
            // List<string> listRoles = new List<string>(account.Roles.Select(x => x.Name));
            // var userRoles = authResult.Principal.FindAll(OpenIddictConstants.Claims.Role).Select(r => r.Value).ToList();
            // if (!listRoles.Contains(Role.admin.ToString()) && !listRoles.Contains(Role.manager.ToString()))
            if (!userRoles.Contains(RoleEnum.admin.ToString()))
            {
                if (accountId != id.ToString())
                {
                    var paramError = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    await _logService.AddLogWebInfo(LogLevelWebInfo.error, "Sửa tài khoản không thành công", paramError);
                    return Unauthorized();
                }
            }
            var result = await _iAccountService.Update(id, model);
            if (result != null && result.GetType() == typeof(ErrorResponseModel))
            {
                return BadRequest(result);
            }
            var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "Sửa tài khoản thành công", paramTrace);
            return Ok(result);
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var account = await _iAccountService.GetByIdAsync(id);
            var accountLogin = (Account)HttpContext.Items[ConstantsInternal.Account];
            // nếu tài khoản null thì trả về badrequest
            if (account == null)
            {
                await _logService.AddLogWebInfo(LogLevelWebInfo.error, "Gặp lỗi khi xóa tài khoản, tài khoản null", id.ToString());
                return BadRequest();
            }
            // không được xóa tài khoản admin
            List<string> listRoles = new List<string>(account.Roles.Select(x => x.Name));
            if (account != null && listRoles.Contains(RoleEnum.admin.ToString()))
            {
                ErrorResponseModel error = new ErrorResponseModel
                {
                    Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.NotpermissionMessage } } }
                };
                return BadRequest(error);
            }
            // thỏa mãn hết thì cho xóa tài khoản
            await _iAccountService.DeleteAsync(id);
            await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "Xóa tài khoản thành công + ${}", id.ToString());
            return Ok(_mapper.Map<AccountResponse>(account));
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Account), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItem(Guid id)
        {
            // var account = await _iAccountService.GetByIdAsync(id);
            var account = await _dbContext.Accounts.Include(a => a.Roles)
                                                   .FirstOrDefaultAsync(a => a.Id == id);
            return account == null ? BadRequest() : Ok(_mapper.Map<AccountResponse>(account));
        }

        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CountAllAccount()
        {
            var total = await _iAccountService.CountAllAccount();
            return Ok(total);
        }
    }

}