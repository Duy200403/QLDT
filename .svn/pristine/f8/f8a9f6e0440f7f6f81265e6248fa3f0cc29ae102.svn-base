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
    public class MenuItemRepository : GenericRepository<MenuItem>, ICommonRepository
    {
        public MenuItemRepository(ApplicationDbContext context, ILogger<MenuItem> logger) : base(context, logger)
        {

        }

        public override async Task<MenuItem> UpsertAsync(MenuItem entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).Include(m => m.ApiRoleMappings).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                existItem.Title = entity.Title;
                existItem.Path = entity.Path;
                existItem.ParentId = entity.ParentId;

                if (entity.ApiRoleMappings != null && entity.ApiRoleMappings.Count > 0)
                {
                    // 1) Clear old mappings
                    existItem.ApiRoleMappings.Clear();
                    foreach (var apiRoleMapping in entity.ApiRoleMappings)
                    {
                        // Add new mappings
                        if (!existItem.ApiRoleMappings.Any(a => a.Id == apiRoleMapping.Id))
                        {
                            existItem.ApiRoleMappings.Add(apiRoleMapping);
                        }
                    }
                }

                existItem.UpdatedBy = entity.UpdatedBy;
                existItem.UpdatedDate = DateTime.UtcNow;

                return existItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(MenuItemRepository));
                throw ex;
            }
        }
    }
}