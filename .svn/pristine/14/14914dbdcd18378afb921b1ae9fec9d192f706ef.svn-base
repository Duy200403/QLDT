using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using AppApi.Services.Common;
using System.Security.Claims;
using OpenIddict.Validation.AspNetCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;
using Microsoft.Extensions.Logging;

namespace AppApi.Infrastructure.Middleware
{
    public class DynamicRolesRequirement : IAuthorizationRequirement { }

    public class DynamicRolesHandler : AuthorizationHandler<DynamicRolesRequirement>
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DynamicRolesHandler> _logger;

        public DynamicRolesHandler(IRolePermissionService rolePermissionService, IHttpContextAccessor httpContextAccessor)
        {
            _rolePermissionService = rolePermissionService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRolesRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            // 1. Authenticate để check lỗi vì sao : để test
            //     var authResult = await httpContext.AuthenticateAsync(
            // OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            //     if (!authResult.Succeeded || authResult.Principal == null)
            //     {
            //         _logger.LogWarning("Authentication failed, cannot log claims.");
            //         context.Fail();
            //         return;
            //     }

            //     // **Log tất cả claims** từ Principal sau introspection
            //     foreach (var claim in authResult.Principal.Claims)
            //     {
            //         _logger.LogInformation("Claim => Type: {Type}, Value: {Value}",
            //             claim.Type, claim.Value);
            //     }
        
            // 1. Authenticate
            var authResult = await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            if (!authResult.Succeeded || authResult.Principal == null || !authResult.Principal.Identity.IsAuthenticated)
            {
                // Nếu xác thực thất bại, trả về 401 + JSON rồi fail
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var error = authResult.Properties?.Items.TryGetValue(OpenIddictValidationAspNetCoreConstants.Properties.Error, out var e) == true ? e : null;
                var desc  = authResult.Properties?.Items.TryGetValue(OpenIddictValidationAspNetCoreConstants.Properties.ErrorDescription, out var d) == true ? d : null;

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = "Unauthorized",
                    error,
                    error_description = desc
                }));
                context.Fail();
                return;
            }

            var path = httpContext.Request.Path.ToString().ToLowerInvariant();
            var allowedRoles = await _rolePermissionService.GetRolesForRouteAsync(path);

            // var userRoles = context.User.FindAll(OpenIddictConstants.Claims.Role).Select(r => r.Value).ToList();
            var userRoles = authResult.Principal.FindAll(OpenIddictConstants.Claims.Role).Select(r => r.Value).ToList();
            if (userRoles.Any(r => allowedRoles.Contains(r)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }

    public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "DynamicRoles";

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() =>
            Task.FromResult<AuthorizationPolicy>(null);

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, System.StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new DynamicRolesRequirement())
                    .Build();

                return Task.FromResult(policy);
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
