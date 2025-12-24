using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
// using ApiWebsite.Common;
// using ApiWebsite.Core.Base;
using AppApi.Common.Helper;
using AppApi.DataAccess.Base;
using AppApi.DTO.Common;
using AppApi.DTO.Models.RoleDto;
using AppApi.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AppApi.Services.Common
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<string>> GetRolesForRouteAsync(string route);
    }

    public class RolePermissionService : IRolePermissionService
    {
        private readonly ApplicationDbContext _dbContext;
        public RolePermissionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetRolesForRouteAsync(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return Enumerable.Empty<string>();

            route = route.ToLowerInvariant();
            route = Regex.Replace(route, @"^/api/v\d+/", "/");
            var baseRoute = Regex.Match(route, @"^(/[^/]+/[^/]+)").Value;
            var mappings = await _dbContext.ApiRoleMapping
                .Where(x => ("/" + x.Controller + "/" + x.Action).ToLower() == baseRoute)
                .ToListAsync();

            // return mappings
            //     .SelectMany(m => m.LstAllowedRoles) // dùng property mới
            //     .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

             var roleNames = mappings
                // flatten all AllowedRole objects
                .SelectMany(m => m.LstAllowedRoles ?? new List<AllowedRole>())
                // pick just the Name
                .Select(r => r.Name)
                // remove empties & duplicates
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return roleNames;
        }
    }
}