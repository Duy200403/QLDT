using AppApi.DataAccess.Base;
using AppApi.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public sealed class DynamicRoleAttributeManagementSoftware : IAuthorizationRequirement {}

public class DynamicRolesManagementSoftwareHandler : AuthorizationHandler<DynamicRoleAttributeManagementSoftware>
{
    private readonly WebApiDbContext _db;
    private readonly IHttpContextAccessor _http;

    public DynamicRolesManagementSoftwareHandler(WebApiDbContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DynamicRoleAttributeManagementSoftware requirement)
    {
        var http = _http.HttpContext;
        if (http == null) return;

        // Lấy Controller / Action hiện tại
        var cad = http.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (cad is null) return;

        // Tìm mapping trong ApiRoleMapping
        var mapping = await _db.ApiRoleMapping.AsNoTracking()
            .FirstOrDefaultAsync(m => 
                m.Controller == cad.ControllerName && 
                m.Action == cad.ActionName);

        if (mapping is null)
            return;

        // Lấy username từ token (ưu tiên preferred_username)
        var username = context.User.FindFirst("preferred_username")?.Value
                    ?? context.User.Identity?.Name
                    ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? context.User.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(username))
            return;

        // 1️⃣ Lấy role từ token (nếu AuthServer phát)
        var tokenRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Claim "role"
        foreach (var claim in context.User.FindAll("role"))
            tokenRoles.Add(claim.Value);

        // Claim "roles" (nhiều role trong 1 chuỗi)
        var rolesStr = context.User.FindFirst("roles")?.Value;
        if (!string.IsNullOrWhiteSpace(rolesStr))
        {
            foreach (var r in rolesStr.Split(new[] { ',', ';', ' ' },
                     StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                tokenRoles.Add(r);
        }

        // 2️⃣ Lấy role từ bảng AccountGroupRole
        var dbRoles = await _db.AccountGroupRoles
            .Where(x => x.Username == username)
            .Select(x => x.RoleId)
            .Distinct()
            .ToListAsync();

        // Gộp cả 2 nguồn lại
        var userRoles = new HashSet<string>(tokenRoles, StringComparer.OrdinalIgnoreCase);
        foreach (var r in dbRoles)
            userRoles.Add(r);

        // 3️⃣ Lấy danh sách role được phép từ ApiRoleMapping (JSON)
        var allowed = (mapping.LstAllowedRoles ?? new List<AllowedRole>())
            .Select(x => x.Id.ToString())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 4️⃣ Kiểm tra giao nhau giữa userRoles và allowed
        if (allowed.Count > 0 && userRoles.Overlaps(allowed))
        {
            context.Succeed(requirement);  // ✅ Cho phép
        }
    }
}
