using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppApi.Common.Helper;
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
using AppApi.Services.AuthService;
using AppApi.Services.LogServ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;
using System.Net.Http;
using System.Net.Http.Headers;
using AppApi.Services.WebApi;
using AppApi.DTO.Models.Auth;
using AppApi.DTO.Models.Response;
using Microsoft.AspNetCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AppApi.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AppApi.AuthService.Controllers
{
    [Route("connect")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly ILogService _logService;
        private readonly AuthDbContext _dbContext;
        // private IEmailConfigService _iEmailConfigService;
        // private string subjectEmail = "Gửi mã xác thực để lấy lại mật khẩu";
        // private string contentEmail = "Mã xác thực của bạn là {0}";
        public AuthController(AuthDbContext dbContext, ILogger<AuthController> logger, IAuthService authService, ILogService logService, IAccountService accountService)
        {
            _logger = logger;
            _authService = authService;
            _logService = logService;
            // _iEmailConfigService = iEmailConfigService;
            _accountService = accountService;
            _dbContext = dbContext;
        }

        private async Task<bool> SelectUserNameAndPhanMemID(string UserName, string PhanMemID)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PhanMemID))
            {
                // Instantiate the HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Specify the URI to get data from
                    string uri = $"http://108.108.108.52:10808/api/DM_PhanMemLogin/SelectByUserNameAndPhanMemID?_UserName={UserName}&_PhanMemID={PhanMemID}";

                    try
                    {
                        // Send a GET request to the specified Uri
                        HttpResponseMessage response = await client.GetAsync(uri);

                        // Ensure we receive a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response content as a string asynchronously
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // var settings = new JsonSerializerSettings
                        // {
                        //     NullValueHandling = NullValueHandling.Ignore,
                        //     MissingMemberHandling = MissingMemberHandling.Ignore
                        // };

                        // var parsedJsonModel = JsonConvert.DeserializeObject<JsonModelObject>(responseBody, settings);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody).ToString();
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult);
                        if (result != null && result._Data != null && result._Data.Count > 0)
                        {
                            var itemData = result._Data[0];

                            if (itemData.UserName.Value != null && itemData.UserName.Value != "" && itemData.PhanMemID.Value != null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle any errors that occurred during the request
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);

                        return false;
                    }
                }
            }
            return false;
        }

        private async Task<bool> DeleteUserNameAndPhanMemID(string UserName, string PhanMemID)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PhanMemID))
            {
                // Instantiate the HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Specify the URI to get data from
                    string uri = $"http://108.108.108.52:10808/api/DM_PhanMemLogin/DeleteByUserNameAndPhanMemID?_UserName={UserName}&_PhanMemID={PhanMemID}";

                    try
                    {
                        // Send a GET request to the specified Uri
                        HttpResponseMessage response = await client.GetAsync(uri);

                        // Ensure we receive a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response content as a string asynchronously
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // var settings = new JsonSerializerSettings
                        // {
                        //     NullValueHandling = NullValueHandling.Ignore,
                        //     MissingMemberHandling = MissingMemberHandling.Ignore
                        // };

                        // var parsedJsonModel = JsonConvert.DeserializeObject<JsonModelObject>(responseBody, settings);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody).ToString();
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult);
                        var itemData = result._Data[0];

                        if (result != null && result._Data != null && result._Data.Count > 0)
                        {
                            if (itemData.UserName.Value != null && itemData.UserName.Value != "" && itemData.PhanMemID.Value != null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle any errors that occurred during the request
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);

                        return false;
                    }
                }
            }
            return false;
        }

        private async Task<dynamic> SelectEmployeeByUserName(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                // Instantiate the HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Specify the URI to get data from
                    string uri = $"http://108.108.108.52:10808/api/DM_NhanVien/SelectNhanVienByUser?_UserName={UserName}";

                    try
                    {
                        // Send a GET request to the specified Uri
                        HttpResponseMessage response = await client.GetAsync(uri);

                        // Ensure we receive a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response content as a string asynchronously
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // var settings = new JsonSerializerSettings
                        // {
                        //     NullValueHandling = NullValueHandling.Ignore,
                        //     MissingMemberHandling = MissingMemberHandling.Ignore
                        // };

                        // var parsedJsonModel = JsonConvert.DeserializeObject<JsonModelObject>(responseBody, settings);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody).ToString();
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult);
                        if (result != null && result._Data != null && result._Data.Count > 0)
                        {
                            var itemData = result._Data[0];

                            return itemData;

                            // if (itemData.UserName.Value != null && itemData.UserName.Value != "" && itemData.PhanMemID.Value != null)
                            // {
                            //     return true;
                            // }
                            // else
                            // {
                            //     return false;
                            // }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle any errors that occurred during the request
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);

                        return null;
                    }
                }
            }
            return null;
        }

        // [AllowAnonymous]
        // [HttpGet("authorize")]
        // public IActionResult Authorize([FromQuery] OpenIddictRequest request)
        // {
        //     // If you’re developing locally you might proxy to your React dev server:
        //     return Redirect("http://localhost:3000/login" + Request.QueryString);
        // }

        /// <summary>
        /// API của click Sign in SSO để đăng nhập Openiddict hoặc chuyển sang trang Login trên Reactjs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] OpenIddictRequest request)
        {
            var redirectUri = request.RedirectUri ?? Request.Query["redirect_uri"].FirstOrDefault();
            var queryString = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;

            // Kiểm tra user đã đăng nhập chưa
            if (!User.Identity.IsAuthenticated)
            {
                // Chưa login, redirect đến login form của ReactJS
                return Redirect("https://localhost:3000/login" + queryString);
            }
            else
            {
                var account = await _dbContext.Accounts.Include(a => a.Roles).FirstOrDefaultAsync(x => x.Username == User.Identity.Name);
                if (account == null)
                    return Redirect("https://localhost:3000/login" + queryString);
                if (!await ValidateAccountStatus(account))
                    return Redirect("https://localhost:3000/login" + queryString);

                var principal = await BuildClaimsPrincipal(account, request.GetScopes(), HttpContext);

                return SignIn(
                    principal,
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
                );
            }

            // return ????;
        }

        /// <summary>
        /// Login Username và Password với Openiddict (lấy authorization_code) và Cookie
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [AllowAnonymous]
        [HttpPost("authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AuthorizePost()
        {
            // 1) Grab the original OIDC request (client_id, redirect_uri, code_challenge, etc.)
            var oidcRequest = HttpContext.GetOpenIddictServerRequest()
                              ?? throw new InvalidOperationException("Không lấy được OpenIddict request.");

            // 3) Pull username/password from the posted form
            var username = Request.Form["Username"].ToString();
            var password = Request.Form["Password"].ToString();

            // 4) Load user + roles
            var account = await _dbContext.Accounts
                              .Include(a => a.Roles)
                              .FirstOrDefaultAsync(a => a.Username == username);
            if (account == null)
                return Redirect($"{Request.Path}{Request.QueryString}&error=invalid_credentials");

            // 5) (Optional) any extra status checks
            if (!await ValidateAccountStatus(account))
                return Redirect($"{Request.Path}{Request.QueryString}&error=invalid_credentials");

            // 6) Verify password + handle lockout
            if (!BC.Verify(password, account.PasswordHash))
            {
                account.AccessFailedCount++;
                if (account.AccessFailedCount >= 5)
                {
                    account.IsLock = true;
                    account.TimeLock = DateTime.UtcNow.AddMinutes(5);
                }
                await _dbContext.SaveChangesAsync();

                return Redirect($"{Request.Path}{Request.QueryString}&error=invalid_credentials");
            }

            // 7) Successful login ⇒ reset failure counters
            account.AccessFailedCount = 0;
            account.IsLock = false;
            account.TimeLock = null;
            await _dbContext.SaveChangesAsync();

            // 8) Build the OpenIddict claims principal with the requested scopes
            var requestedScopes = oidcRequest.GetScopes();
            var principal = await BuildClaimsPrincipal(account, requestedScopes, HttpContext);

            // 9) Tell OpenIddict to issue the authorization code and redirect back to your SPA
            // return SignIn(
            //     principal,
            //     OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            // );
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal, // claims principal của user
                new AuthenticationProperties { IsPersistent = true }
            );

            // issue the authorization code
            return SignIn(
                principal,
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }

        /// <summary>
        /// Sau đó Sign in token để lấy 3 token: access_token, refresh_token, identity_token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [AllowAnonymous]
        [HttpPost("token"), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                              ?? throw new InvalidOperationException("Không lấy được OpenIddict request.");

            if (request.IsAuthorizationCodeGrantType())
            {
                // Xác thực code + PKCE
                var auth = await HttpContext.AuthenticateAsync(
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                var principal = auth.Principal!;

                // (tuỳ chọn) bạn có thể inspect principal để kiểm tra custom claims

                // Issue tokens
                return SignIn(
                    principal,
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // **ĐÂY**: vẫn phải load user, ValidateAccountStatus và BuildClaimsPrincipal
                var authenticateResult = await HttpContext
                  .AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                var oldPrincipal = authenticateResult.Principal!;
                var userId = oldPrincipal.GetClaim(Claims.Subject);
                var account = await _dbContext.Accounts
                                                .Include(a => a.Roles)
                                                .FirstOrDefaultAsync(a => a.Id.ToString() == userId);

                // ValidateAccountStatus trước khi cấp token mới
                if (account == null
                 || !await ValidateAccountStatus(account))
                    return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Tạo principal mới với cùng scopes
                var scopes = oldPrincipal.GetScopes();
                var newPrincipal = await BuildClaimsPrincipal(account, scopes, HttpContext);

                return SignIn(
                  newPrincipal,
                //   authenticateResult.Properties,
                  OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return BadRequest(new { error = "unsupported_grant_type" });
        }

        private async Task<bool> ValidateAccountStatus(
                Account account)
        {
            if (!account.IsActive)
            {
                await _logService.AddLogWebInfo(LogLevelWebInfo.error,
                    "Login", "Tài khoản chưa active");
                // for auth‐code you must surface the error via redirect
                return false;
            }
            if (account.IsLock && account.TimeLock.HasValue &&
                DateTime.UtcNow < account.TimeLock.Value)
            {
                return false;
            }
            return true;
        }

        private static async Task<ClaimsPrincipal> BuildClaimsPrincipal(
                Account account,
                IEnumerable<string> scopes,
                HttpContext httpContext)
        {
            // Tạo identity dùng scheme của OpenIddict Server
            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);

            // Thêm các claim cơ bản
            identity.AddClaim(Claims.Subject, account.Id.ToString());
            identity.AddClaim(Claims.Name, account.FullName ?? string.Empty);
            identity.AddClaim(Claims.PreferredUsername, account.Username ?? string.Empty);

            var db = httpContext.RequestServices.GetRequiredService<AuthDbContext>();

            var allRoleIds = await GetAllRoleAncestorsAsync(account.Roles.Select(r => r.Id), db);

            // Lấy tên tương ứng để phát cả Id và Name
            var allRoles = await db.Roles
                .Where(r => allRoleIds.Contains(r.Id))
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            foreach (var r in allRoles)
            {
                identity.AddClaim(Claims.Role, r.Name);      // role name
                identity.AddClaim("role_id", r.Id.ToString()); // role id
            }

            // Quyết định claim nào cho access_token và/hoặc identity_token
            identity.SetDestinations(claim =>
            {
                return claim.Type switch
                {
                    // selalu cần trong access_token để API đọc via introspection
                    Claims.Subject => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    Claims.Name => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    Claims.PreferredUsername => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    Claims.Role => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    "role_id" => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    _ => Array.Empty<string>()
                };
            });


            // Tạo principal và gán scope
            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(scopes);

            var resources = await GetClientIdsAsync(
                httpContext.RequestServices.GetRequiredService<IOpenIddictApplicationManager>());

            var request = httpContext.GetOpenIddictServerRequest();
            if (request is not null)
            {
                // tell OpenIddict which resource servers this token can access:
                principal.SetResources(resources);

                // (optional) also set audiences if you need JwtAudience checks:
                // principal.SetAudiences(scopes);
            }

            return principal;
        }

        private static async Task<HashSet<Guid>> GetAllRoleAncestorsAsync(
            IEnumerable<Guid> roleIds,
            AuthDbContext db)
        {
            var all = new HashSet<Guid>(roleIds);
            if (!all.Any()) return all;

            var map = await db.Roles
                .Select(r => new { r.Id, r.ParentId })
                .ToListAsync();

            var lookup = map.ToDictionary(x => x.Id, x => x.ParentId);

            var stack = new Stack<Guid>(roleIds);
            while (stack.Count > 0)
            {
                var id = stack.Pop();
                if (lookup.TryGetValue(id, out var parent) && parent.HasValue && all.Add(parent.Value))
                    stack.Push(parent.Value);
            }

            return all;
        }

        private static async Task<List<string>> GetClientIdsAsync(
        IOpenIddictApplicationManager appManager,
        int maxDegreeOfParallelism = 10)
        {
            var clientIds = new List<string>();
            var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
            var tasks = new List<Task>();

            await foreach (var app in appManager.ListAsync())
            {
                await semaphore.WaitAsync();

                var task = Task.Run(async () =>
                {
                    try
                    {
                        var clientId = await appManager.GetClientIdAsync(app);
                        lock (clientIds)
                        {
                            clientIds.Add(clientId);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            return clientIds;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string post_logout_redirect_uri)
        {
            // Xoá cookie xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect về lại SPA (hoặc trang login)
            return Redirect(post_logout_redirect_uri ?? "/");
        }

        // [HttpGet("endsession")]
        // public async Task<IActionResult> Logout()
        // {
        //     // Lấy yêu cầu end-session từ OpenIddict
        //     var context = HttpContext.GetOpenIddictServerRequest();
        //     if (context == null)
        //     {
        //         return BadRequest("Invalid OpenID Connect request.");
        //     }

        //     // Xóa cookie xác thực
        //     await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //     // Lấy post_logout_redirect_uri từ yêu cầu
        //     var redirectUri = context.PostLogoutRedirectUri;
        //     if (string.IsNullOrEmpty(redirectUri))
        //     {
        //         return Redirect("/"); // Trang mặc định nếu không có URI
        //     }

        //     // Kiểm tra tính hợp lệ của redirectUri (tùy chọn, vì OpenIddict đã kiểm tra)
        //     // Bạn có thể thêm logic kiểm tra bổ sung nếu cần
        //     return Redirect(redirectUri);
        // }

        // [HttpGet("authorize")]
        // public async Task<IActionResult> Authorize()
        // {
        //     var context = HttpContext.GetOpenIddictServerRequest() ??
        //         throw new InvalidOperationException("Không lấy được yêu cầu OpenIddict.");

        //     // Kiểm tra nếu người dùng đã đăng nhập
        //     if (User.Identity.IsAuthenticated)
        //     {
        //         var principal = new ClaimsPrincipal(User.Identity);
        //         principal.SetScopes(context.GetScopes());
        //         principal.SetResources(context.ClientId);

        //         var authProperties = new AuthenticationProperties
        //         {
        //             RedirectUri = context.RedirectUri
        //         };

        //         return SignIn(principal, authProperties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //     }

        //     // Hiển thị form đăng nhập
        //     ViewData["ClientId"] = context.ClientId;
        //     ViewData["RedirectUri"] = context.RedirectUri;
        //     ViewData["Scope"] = context.Scope;
        //     ViewData["CodeChallenge"] = context.CodeChallenge;
        //     return View("Login");
        // }

        // [HttpPost("authorize")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Authorize(string username, string password, string clientId, string redirectUri, string scope, string codeChallenge)
        // {
        //     var context = HttpContext.GetOpenIddictServerRequest() ??
        //         throw new InvalidOperationException("Không lấy được yêu cầu OpenIddict.");

        //     var account = await _dbContext.Accounts
        //         .Include(a => a.Roles)
        //         .FirstOrDefaultAsync(a => a.Username == username);

        //     if (account == null || !BC.Verify(password, account.PasswordHash))
        //     {
        //         ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
        //         ViewData["ClientId"] = clientId;
        //         ViewData["RedirectUri"] = redirectUri;
        //         ViewData["Scope"] = scope;
        //         ViewData["CodeChallenge"] = codeChallenge;
        //         return View("Login");
        //     }

        //     if (!account.IsActive || (account.IsLock && account.TimeLock.HasValue && DateTime.UtcNow < account.TimeLock.Value))
        //     {
        //         ModelState.AddModelError("", "Tài khoản không hợp lệ hoặc đang bị khóa.");
        //         ViewData["ClientId"] = clientId;
        //         ViewData["RedirectUri"] = redirectUri;
        //         ViewData["Scope"] = scope;
        //         ViewData["CodeChallenge"] = codeChallenge;
        //         return View("Login");
        //     }

        //     // Reset trạng thái khóa
        //     account.AccessFailedCount = 0;
        //     account.IsLock = false;
        //     account.TimeLock = null;
        //     await _dbContext.SaveChangesAsync();

        //     // Tạo ClaimsPrincipal
        //     var identity = new ClaimsIdentity(
        //         OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
        //         Claims.Name,
        //         Claims.Role);

        //     identity.AddClaim(Claims.Subject, account.Id.ToString());
        //     identity.AddClaim(Claims.Name, account.FullName ?? string.Empty);
        //     identity.AddClaim(Claims.PreferredUsername, account.Username ?? string.Empty);

        //     foreach (var role in account.Roles)
        //     {
        //         identity.AddClaim(Claims.Role, role.Name);
        //     }

        //     var principal = new ClaimsPrincipal(identity);
        //     principal.SetScopes(context.GetScopes());
        //     principal.SetResources(clientId);

        //     var authProperties = new AuthenticationProperties
        //     {
        //         RedirectUri = redirectUri
        //     };

        //     return SignIn(principal, authProperties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        // }

        [Authorize(Policy = "DynamicRoles")]
        [HttpGet("userinfo")]
        public IActionResult UserInfo()
        {
            // cái này có thể lấy từ bảng AccountInfo? 
            // Cũng ko hợp lý lắm vì AccountInfo nên để trong WebApi
            return Ok(new
            {
                sub = User.FindFirstValue(Claims.Subject),
                name = User.FindFirstValue(Claims.Name),
                preferred_username = User.FindFirstValue(Claims.PreferredUsername),
                roles = User.FindAll(Claims.Role).Select(c => c.Value)
            });
        }


        /// <summary>
        /// Phần dưới này bỏ qua
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // [HttpPost("authenticate")]
        // public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        // {
        //     var response = await _authService.Authenticate(model, ipAddress());
        //     if (response == null)
        //     {
        //         var paramError = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //         await _logService.AddLogWebInfo(LogLevelWebInfo.error, "Đăng nhập thất bại, Unauthorized", paramError);
        //         return Unauthorized();
        //     }
        //     if (response.GetType() == typeof(ErrorResponseModel))
        //     {
        //         return BadRequest(response);
        //     }
        //     return Ok(response);
        // }

        // [HttpPost("refreshToken")]
        // public async Task<IActionResult> RefreshToken([FromBody] AuthenticateRefreshTokenRequest model)
        // {
        //     var response = await _authService.RefreshToken(model);
        //     if (response.GetType() == typeof(ErrorResponseModel))
        //     {
        //         return BadRequest(response);
        //     }
        //     return Ok(response);
        // }

        // // [Authorize(RoleEnum.admin, RoleEnum.doctor)]
        // [Authorize]
        // [HttpPut("[action]")]
        // [MapToApiVersion("1.0")]
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        // {
        //     var account = (Account)HttpContext.Items[ConstantsInternal.Account];
        //     var response = await _authService.ChangePassword(request, account);
        //     if (response.GetType() == typeof(ErrorResponseModel))
        //     {
        //         return BadRequest(response);
        //     }
        //     var paramTrace = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        //     await _logService.AddLogWebInfo(LogLevelWebInfo.error, "Thay đổi mật khẩu thành công", paramTrace);
        //     return Ok();
        // }
        // [HttpPost("[action]")]
        // [MapToApiVersion("1.0")]
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        // {
        //     var account = await _accountService.GetOneAsync(x => x.Email == request.Email);
        //     // check email để gửi code có tồn tại hay không
        //     if (account is null)
        //     {
        //         ErrorResponseModel error = new ErrorResponseModel
        //         {
        //             Type = typeError.Email.ToString(),
        //             Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.EmailNotFoundMessage } } }
        //         };

        //         var paramError = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        //         await _logService.AddLogWebInfo(LogLevelWebInfo.error, "AuthController, ForgotPassword, Email không tồn tại", paramError);
        //         return BadRequest(error);
        //     }
        //     // check VerifacationCode có dữ liệu, check xem mã xác thực còn hiệu lực không
        //     if (account.VerifacationCode != null)
        //     {
        //         var objVC = Newtonsoft.Json.JsonConvert.DeserializeObject<VerificationCode>(account.VerifacationCode);
        //         if (objVC.ExpiredAt > DateTime.UtcNow && objVC.IsUsed == false)
        //         {
        //             ErrorResponseModel error = new ErrorResponseModel
        //             {
        //                 Type = typeError.Code.ToString(),
        //                 Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.VerificationCodeStillValid } } }
        //             };

        //             var paramError = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        //             await _logService.AddLogWebInfo(LogLevelWebInfo.error, "AuthController, ForgotPassword, Mã xác thực còn hiệu lực", paramError);
        //             return BadRequest(error);
        //         }
        //     }
        //     Random _random = new Random();
        //     VerificationCode VC = new VerificationCode()
        //     {
        //         IsUsed = false,
        //         Code = _random.Next(100000, 999999).ToString(),
        //         ExpiredAt = DateTime.UtcNow.AddMinutes(5)
        //     };
        //     account.VerifacationCode = Newtonsoft.Json.JsonConvert.SerializeObject(VC);
        //     await _accountService.CompleteServiceAsync();
        //     // await Util.SendEmail(_iEmailConfigService, subjectEmail, request.Email, string.Format(contentEmail, VC.Code));
        //     return Ok();
        // }
        // [HttpPost("[action]")]
        // [MapToApiVersion("1.0")]
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        // {
        //     var account = await _accountService.GetOneAsync(x => x.Email == request.Email);
        //     if (account is null)
        //     {
        //         ErrorResponseModel error = new ErrorResponseModel
        //         {
        //             Type = typeError.Email.ToString(),
        //             Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.EmailNotFoundMessage } } }
        //         };
        //         var paramError = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        //         await _logService.AddLogWebInfo(LogLevelWebInfo.error, "AuthController, ResetPassword, Email không tồn tại", paramError);
        //         return BadRequest(error);
        //     }
        //     if (account.VerifacationCode != null)
        //     {
        //         var objVC = Newtonsoft.Json.JsonConvert.DeserializeObject<VerificationCode>(account.VerifacationCode);
        //         if (objVC.Code == request.Code && objVC.ExpiredAt >= DateTime.UtcNow && objVC.IsUsed == false)
        //         {
        //             objVC.IsUsed = true;
        //             account.PasswordHash = BC.HashPassword(request.Password);
        //             account.Salt = BC.GenerateSalt();
        //             // account.VerifacationCode = Newtonsoft.Json.JsonConvert.SerializeObject(objVC);
        //             account.VerifacationCode = null;
        //             await _accountService.CompleteServiceAsync();
        //             await _logService.AddLogWebInfo(LogLevelWebInfo.trace, "AuthController, ResetPassword, Lấy lại mật khẩu thành công", "Succses");
        //             return Ok();
        //         }
        //         if (objVC.Code != request.Code)
        //         {
        //             ErrorResponseModel error = new ErrorResponseModel
        //             {
        //                 Type = typeError.Code.ToString(),
        //                 Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.VerificationCodeExactly } } }
        //             };
        //             return BadRequest(error);
        //         }
        //         if (objVC.ExpiredAt <= DateTime.UtcNow && objVC.IsUsed == false)
        //         {
        //             ErrorResponseModel error = new ErrorResponseModel
        //             {
        //                 Type = typeError.Code.ToString(),
        //                 Errors = new Dictionary<string, string[]> { { Enum.GetName(typeof(ErrorModelPropertyName), ErrorModelPropertyName.content), new string[] { ConstantsInternal.VerificationCodeExp } } }
        //             };
        //             return BadRequest(error);
        //         }
        //     }
        //     return BadRequest();

        // }
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}