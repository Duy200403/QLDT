using System;
using System.Linq;
using System.Threading.Tasks;
using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppApi.DataAccess.Repositories.Common
{
    public class ApiRoleMappingRepository : GenericRepository<ApiRoleMapping>, ICommonRepository
    {
        public ApiRoleMappingRepository(ApplicationDbContext context, ILogger<ApiRoleMapping> logger) : base(context, logger)
        {

        }

        public override async Task<ApiRoleMapping> UpsertAsync(ApiRoleMapping entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                existItem.Controller = entity.Controller;
                existItem.Action = entity.Action;
                existItem.AllowedRoles = entity.AllowedRoles;

                existItem.UpdatedBy = entity.UpdatedBy;
                existItem.UpdatedDate = DateTime.UtcNow;

                return existItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(ApiRoleMappingRepository));
                throw ex;
            }
        }
    }
}