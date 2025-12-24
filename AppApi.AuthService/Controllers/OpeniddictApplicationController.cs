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
using OpenIddict.Abstractions;
using AppApi.DTO.Models.OpeniddictRegistration;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AppApi.Common.Model;
using Microsoft.Extensions.Options;
using AppApi.DataAccess.Base;

namespace AppApi.AuthService.Controllers
{
    public class OpeniddictApplicationController : BaseController
    {
        private readonly ILogger<OpeniddictApplicationController> _logger;
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly AuthDbContext _dbContext;
        private readonly IOpenIddictApplicationManager _appManager;
        private readonly IOpenIddictScopeManager _scopeManager;

        public OpeniddictApplicationController(IOpenIddictApplicationManager appManager,
        IOpenIddictScopeManager scopeManager, AuthDbContext dbContext, ILogger<OpeniddictApplicationController> logger,
        IMapper mapper, IRoleService roleService, ILogService logService)
        {
            _logger = logger;
            _roleService = roleService;
            _logService = logService;
            _mapper = mapper;
            _dbContext = dbContext;

            _appManager = appManager;
            _scopeManager = scopeManager;
        }


        /// <summary>
        /// Tạo mới ứng dụng Client + API và đăng ký scopes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] RegisterAppWithScopeRequest model)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                if (await _appManager.FindByClientIdAsync(model.ClientAppId) is not null)
                    return BadRequest(new { message = "Ứng dụng Client đã tồn tại." });

                if (await _appManager.FindByClientIdAsync(model.ApiAppId) is not null)
                    return BadRequest(new { message = "Ứng dụng API đã tồn tại." });

                if (model.CustomScope == null || string.IsNullOrEmpty(model.CustomScope.ScopeName))
                    return BadRequest(new { message = "Chưa thêm quyền cho ứng dụng." });

                if (await _scopeManager.FindByNameAsync(model.CustomScope.ScopeName) is not null)
                    return BadRequest(new { message = $"Quyền '{model.CustomScope.ScopeName}' đã tồn tại." });

                // 1. Đăng ký CLIENT App (e.g. React SPA)
                // if (await appManager.FindByClientIdAsync(model.ClientAppId) is null)
                // {
                var clientApp = new OpenIddictApplicationDescriptor
                {
                    ClientId = model.ClientAppId,
                    DisplayName = model.ClientAppDisplayName,
                    ClientType = ClientTypes.Public,
                };

                // foreach (var uri in model.RedirectUris)
                //     clientApp.RedirectUris.Add(new Uri(uri));

                // foreach (var uri in model.PostLogoutRedirectUris)
                //     clientApp.PostLogoutRedirectUris.Add(new Uri(uri));

                if (!model.ClientDomain.EndsWith("/"))
                {
                    model.ClientDomain = model.ClientDomain + "/";
                }

                clientApp.RedirectUris.Add(
                       new Uri(model.ClientDomain + "callback"));
                clientApp.RedirectUris.Add(new Uri(model.ClientDomain + "silent-renew.html"));
                clientApp.PostLogoutRedirectUris.Add(
                    new Uri(model.ClientDomain));

                clientApp.Permissions.Add(Permissions.Endpoints.Authorization);
                clientApp.Permissions.Add(Permissions.Endpoints.Token);
                clientApp.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
                clientApp.Permissions.Add(Permissions.GrantTypes.RefreshToken);
                clientApp.Permissions.Add(Permissions.ResponseTypes.Code);

                clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                clientApp.Permissions.Add(Permissions.Prefixes.Scope + model.CustomScope.ScopeName);

                // foreach (var s in model.CustomScopes)
                //     clientApp.Permissions.Add(Permissions.Prefixes.Scope + s.Name);

                await _appManager.CreateAsync(clientApp);
                // }
                // else
                // {
                //     return BadRequest(new { message = "Client app already exists." });
                // }

                // // 2. Đăng ký API App (resource = API)
                // if (await appManager.FindByClientIdAsync(model.ApiAppId) is null)
                // {
                var apiApp = new OpenIddictApplicationDescriptor
                {
                    ClientId = model.ApiAppId,
                    ClientSecret = model.ApiAppSecret,
                    DisplayName = model.ApiAppDisplayName,
                    ClientType = ClientTypes.Confidential
                };

                apiApp.Permissions.Add(Permissions.Endpoints.Token);
                apiApp.Permissions.Add(Permissions.Endpoints.Authorization);
                apiApp.Permissions.Add(Permissions.Endpoints.Introspection);
                apiApp.Permissions.Add(Permissions.Endpoints.Revocation);
                apiApp.Permissions.Add(Permissions.GrantTypes.RefreshToken);

                apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                apiApp.Permissions.Add(Permissions.Prefixes.Scope + model.CustomScope.ScopeName);

                // foreach (var s in model.CustomScopes)
                //     apiApp.Permissions.Add(Permissions.Prefixes.Scope + s.Name);

                await _appManager.CreateAsync(apiApp);
                // }
                // else
                // {
                //     return BadRequest(new { message = "API app already exists." });
                // }

                // 3. Đăng ký scopes nếu chưa có

                var scopeDesc = new OpenIddictScopeDescriptor
                {
                    Name = model.CustomScope.ScopeName,
                    DisplayName = model.CustomScope.ScopeDisplayName
                };

                scopeDesc.Resources.Add(model.ApiAppId); // Gắn scope với API app

                await _scopeManager.CreateAsync(scopeDesc);

                // foreach (var s in model.CustomScopes)
                // {
                //     // if (await scopeManager.FindByNameAsync(s.Name) is not null)
                //     //     continue;

                //     var scopeDesc = new OpenIddictScopeDescriptor
                //     {
                //         Name = s.Name,
                //         DisplayName = s.DisplayName
                //     };

                //     scopeDesc.Resources.Add(model.ApiAppId); // Gắn scope với API app

                //     await scopeManager.CreateAsync(scopeDesc);
                // }

                var standardScopes = new[]
                    {
                    Scopes.OpenId,        // "openid"
                    Scopes.Profile,       // "profile"
                    Scopes.Roles,         // "roles"
                    Scopes.OfflineAccess  // "offline_access"
                };

                foreach (var name in standardScopes)
                {
                    // find the persisted scope
                    var scopeEdit = await _scopeManager.FindByNameAsync(name);

                    if (scopeEdit is null)
                        continue;

                    // if it *does* exist, build a descriptor pre-populated from it
                    var descriptorEdit = new OpenIddictScopeDescriptor
                    {
                        Name = name,
                        DisplayName = name
                    };

                    // copy *all* existing resources
                    foreach (var resource in await _scopeManager.GetResourcesAsync(scopeEdit))
                        descriptorEdit.Resources.Add(resource);

                    // add your new one if missing
                    if (!descriptorEdit.Resources.Contains(model.ApiAppId))
                        descriptorEdit.Resources.Add(model.ApiAppId);

                    // finally, apply the update
                    await _scopeManager.UpdateAsync(scopeEdit, descriptorEdit);
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Server error", detail = ex.Message });
            }

            return Ok(new { message = "Client app + API app + scopes tạo thành công." });
        }

        /// <summary>
        /// Lấy thông tin ứng dụng OpenIddict theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            // 1) Fetch the raw application entity
            var application = await _appManager.FindByIdAsync(id.ToString());
            if (application is null)
                return NotFound(new { message = "Application not found." });

            // 2) Populate a descriptor so we can read its properties
            var descriptor = new OpenIddictApplicationDescriptor();
            await _appManager.PopulateAsync(descriptor, application);

            // 3) Build your DTO
            var model = new RegisterAppWithScopeRequest
            {
                IsClientApp = descriptor.ClientType == ClientTypes.Public,
                ClientAppId = descriptor.ClientType == ClientTypes.Public ? descriptor.ClientId : null,
                ClientAppDisplayName = descriptor.ClientType == ClientTypes.Public ? descriptor.DisplayName : null,
                ClientDomain = descriptor.ClientType == ClientTypes.Public
                                            ? ExtractDomain(descriptor.RedirectUris) : null,

                ApiAppId = descriptor.ClientType != ClientTypes.Public ? descriptor.ClientId : null,
                ApiAppDisplayName = descriptor.ClientType != ClientTypes.Public ? descriptor.DisplayName : null,
                // Note: you cannot read back a hashed secret, so leave ApiAppSecret null
            };

            // 4) Pull out all scopes the app currently has
            var permissions = await _appManager.GetPermissionsAsync(application);
            var scopePrefix = Permissions.Prefixes.Scope;

            // standard OIDC scopes you always add:
            var standardScopes = new[]
            {
                Scopes.OpenId,
                Scopes.Profile,
                Scopes.Roles,
                Scopes.OfflineAccess
            };

            // Find the one custom scope (everything beyond the four standard ones)
            var customScopeName = permissions
                .Where(p => p.StartsWith(scopePrefix))
                .Select(p => p.Substring(scopePrefix.Length))
                .FirstOrDefault(s => !standardScopes.Contains(s));

            if (!string.IsNullOrEmpty(customScopeName))
            {
                model.CustomScope.ScopeName = customScopeName;

                // load its display name from the scope manager
                var scopeEntity = await _scopeManager.FindByNameAsync(customScopeName);
                model.CustomScope.ScopeDisplayName =
                    scopeEntity is null
                        ? customScopeName
                        : await _scopeManager.GetDisplayNameAsync(scopeEntity);
            }

            return Ok(model);
        }

        private static string ExtractDomain(ICollection<Uri> callbacks)
        {
            var first = callbacks.FirstOrDefault(uri =>
                uri.AbsoluteUri.EndsWith("/callback") ||
                uri.AbsoluteUri.EndsWith("/silent-renew.html"));

            if (first is null)
                return null;

            var str = first.AbsoluteUri;
            var idx = str.IndexOf("/", str.IndexOf("://", StringComparison.Ordinal) + 3);
            return idx < 0 ? str : str[..(idx + 1)];
        }

        /// <summary>
        /// Lấy danh sách ứng dụng Openiddict với phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging([FromQuery] OpeniddictApplicationPagingFilter request)
        {
            var scope = HttpContext.RequestServices.CreateScope();
            var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var skip = (request.PageIndex - 1) * request.PageSize;
            var count = request.PageSize;

            var result = new List<object>();

            await foreach (var app in appManager.ListAsync(count: count, offset: skip))
            {
                var descriptor = new OpenIddictApplicationDescriptor();
                await appManager.PopulateAsync(descriptor, app);

                var id = app.GetType().GetProperty("Id")?.GetValue(app)?.ToString();

                result.Add(new
                {
                    Id = id,
                    descriptor.ClientId,
                    descriptor.DisplayName,
                    descriptor.ClientType,
                    descriptor.Permissions,
                });
            }

            // Tổng số bản ghi (nếu cần tính total cho pagination client)
            var total = await appManager.CountAsync();

            return Ok(new
            {
                TotalRecords = total,
                request.PageSize,
                request.PageIndex,
                items = result
            });
        }

        /// <summary>
        /// Sửa thôn gtin ứng dụng Openiddict theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, RegisterAppWithScopeRequest model)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var username = HttpContext.User.FindFirst(ConstantsInternal.PreferredUsername)?.Value;

                var itemExist = await _appManager.FindByIdAsync(id.ToString());


                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { message = "Không xác định được người dùng." });
                }
                if (itemExist == null)
                {
                    return NotFound(new { message = "Application not found." });
                }

                if (model.CustomScope == null || string.IsNullOrEmpty(model.CustomScope.ScopeName))
                    return BadRequest(new { message = "Chưa thêm quyền cho ứng dụng." });

                if (model.IsClientApp)
                {
                    var clientApp = new OpenIddictApplicationDescriptor
                    {
                        ClientId = model.ClientAppId,
                        DisplayName = model.ClientAppDisplayName,
                        ClientType = ClientTypes.Public,
                    };

                    if (!model.ClientDomain.EndsWith("/"))
                    {
                        model.ClientDomain = model.ClientDomain + "/";
                    }

                    clientApp.RedirectUris.Add(
                           new Uri(model.ClientDomain + "callback"));
                    clientApp.RedirectUris.Add(new Uri(model.ClientDomain + "silent-renew.html"));
                    clientApp.PostLogoutRedirectUris.Add(
                        new Uri(model.ClientDomain));

                    clientApp.Permissions.Add(Permissions.Endpoints.Authorization);
                    clientApp.Permissions.Add(Permissions.Endpoints.Token);
                    clientApp.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
                    clientApp.Permissions.Add(Permissions.GrantTypes.RefreshToken);
                    clientApp.Permissions.Add(Permissions.ResponseTypes.Code);

                    clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                    clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                    clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                    clientApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                    clientApp.Permissions.Add(Permissions.Prefixes.Scope + model.CustomScope.ScopeName);

                    // foreach (var s in model.CustomScopes)
                    //     clientApp.Permissions.Add(Permissions.Prefixes.Scope + s.Name);

                    await _appManager.PopulateAsync(itemExist, clientApp);
                    await _appManager.UpdateAsync(itemExist);
                }
                else
                {
                    var apiApp = new OpenIddictApplicationDescriptor
                    {
                        ClientId = model.ApiAppId,
                        DisplayName = model.ApiAppDisplayName,
                        ClientType = ClientTypes.Confidential
                    };

                    if (!string.IsNullOrEmpty(model.ApiAppSecret))
                    {
                        apiApp.ClientSecret = model.ApiAppSecret;
                    }
                    else
                    {
                        apiApp.ClientSecret = itemExist.GetType().GetProperty("ClientSecret")?.GetValue(itemExist)?.ToString();
                    }

                    apiApp.Permissions.Add(Permissions.Endpoints.Token);
                    apiApp.Permissions.Add(Permissions.Endpoints.Authorization);
                    apiApp.Permissions.Add(Permissions.Endpoints.Introspection);
                    apiApp.Permissions.Add(Permissions.Endpoints.Revocation);
                    apiApp.Permissions.Add(Permissions.GrantTypes.RefreshToken);

                    apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                    apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                    apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                    apiApp.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                    apiApp.Permissions.Add(Permissions.Prefixes.Scope + model.CustomScope.ScopeName);

                    // foreach (var s in model.CustomScopes)
                    //     apiApp.Permissions.Add(Permissions.Prefixes.Scope + s.Name);

                    await _appManager.PopulateAsync(itemExist, apiApp);
                    await _appManager.UpdateAsync(itemExist);
                }

                var scopeEntity = await _scopeManager.FindByNameAsync(model.CustomScope.ScopeName);
                if (scopeEntity is null)
                {
                    return BadRequest(new { message = $"Scope '{model.CustomScope.ScopeName}' không tồn tại." });
                }

                // Nếu đã tồn tại, cập nhật lại Resource (nếu cần)
                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = model.CustomScope.ScopeName,
                    DisplayName = model.CustomScope.ScopeDisplayName
                };

                foreach (var res in await _scopeManager.GetResourcesAsync(scopeEntity))
                    descriptor.Resources.Add(res);

                if (!descriptor.Resources.Contains(model.ApiAppId))
                    descriptor.Resources.Add(model.ApiAppId);

                await _scopeManager.UpdateAsync(scopeEntity, descriptor);

                // foreach (var s in model.CustomScopes)
                // {
                //     var scopeEntity = await scopeManager.FindByNameAsync(s.Name);
                //     if (scopeEntity is null)
                //     {
                //         return BadRequest(new { message = $"Scope '{s.Name}' không tồn tại." });
                //     }

                //     // Nếu đã tồn tại, cập nhật lại Resource (nếu cần)
                //     var descriptor = new OpenIddictScopeDescriptor
                //     {
                //         Name = s.Name,
                //         DisplayName = s.DisplayName
                //     };

                //     foreach (var res in await scopeManager.GetResourcesAsync(scopeEntity))
                //         descriptor.Resources.Add(res);

                //     if (!descriptor.Resources.Contains(model.ApiAppId))
                //         descriptor.Resources.Add(model.ApiAppId);

                //     await scopeManager.UpdateAsync(scopeEntity, descriptor);
                // }

                var standardScopes = new[]
                {
                Scopes.OpenId,
                Scopes.Profile,
                Scopes.Roles,
                Scopes.OfflineAccess
            };

                foreach (var name in standardScopes)
                {
                    var scopeEntityStandard = await _scopeManager.FindByNameAsync(name);
                    if (scopeEntityStandard is null)
                        return BadRequest(new { message = $"Scope '{name}' không tồn tại." });

                    var descriptorStandard = new OpenIddictScopeDescriptor
                    {
                        Name = name,
                        DisplayName = name
                    };

                    foreach (var res in await _scopeManager.GetResourcesAsync(scopeEntityStandard))
                        descriptorStandard.Resources.Add(res);

                    if (!descriptorStandard.Resources.Contains(model.ApiAppId))
                        descriptorStandard.Resources.Add(model.ApiAppId);

                    await _scopeManager.UpdateAsync(scopeEntityStandard, descriptorStandard);
                }


                // Cập nhật thông tin ứng dụng

                await transaction.CommitAsync();

                var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "OpeniddictAppicationController, Update, OK", paramTrace);
                return Ok(new { message = "Client app + API app + scopes cập nhật thành công." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Server error", detail = ex.Message });
            }
        }

        /// <summary>
        /// Xoá ứng dụng Openiddict và các scopes liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        [Authorize(Policy = "DynamicRoles")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 1) Load the app entity
                var appEntity = await _appManager.FindByIdAsync(id.ToString());
                if (appEntity is null)
                    return NotFound(new { message = "Application not found." });

                // 2) Figure out the client_id and its scopes
                var clientId = await _appManager.GetClientIdAsync(appEntity);
                var permissions = await _appManager.GetPermissionsAsync(appEntity);

                // strip off the "scope:" prefix
                var allScopes = permissions
                    .Where(p => p.StartsWith(Permissions.Prefixes.Scope))
                    .Select(p => p.Substring(Permissions.Prefixes.Scope.Length))
                    .ToList();

                // the four standard OIDC scopes
                var standardScopes = new[]
                {
                    Scopes.OpenId,
                    Scopes.Profile,
                    Scopes.Roles,
                    Scopes.OfflineAccess
                };

                // 3) Delete any custom scopes (everything *except* the standard ones)
                var customScopes = allScopes.Except(standardScopes);
                foreach (var name in customScopes)
                {
                    var scopeEntity = await _scopeManager.FindByNameAsync(name);
                    if (scopeEntity is not null)
                        await _scopeManager.DeleteAsync(scopeEntity);
                }

                // 4) Remove this clientId from the standard scopes’ resources
                foreach (var name in standardScopes)
                {
                    var scopeEntity = await _scopeManager.FindByNameAsync(name);
                    if (scopeEntity is null)
                        continue;

                    // build a fresh descriptor from the existing scope
                    var descriptor = new OpenIddictScopeDescriptor
                    {
                        Name = name,
                        DisplayName = name
                    };

                    // copy over every existing resource
                    foreach (var res in await _scopeManager.GetResourcesAsync(scopeEntity))
                        descriptor.Resources.Add(res);

                    // remove our clientId if present
                    if (descriptor.Resources.Remove(clientId))
                        await _scopeManager.UpdateAsync(scopeEntity, descriptor);
                }

                // 5) Now finally delete the application
                await _appManager.DeleteAsync(appEntity);

                await transaction.CommitAsync();

                var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(appEntity);
                await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "OpeniddictAppicationController, Delete, OK", paramTrace);

                return Ok(new { message = "Xóa application và scope thành công." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Server error", detail = ex.Message });
            }
        }
    }
}