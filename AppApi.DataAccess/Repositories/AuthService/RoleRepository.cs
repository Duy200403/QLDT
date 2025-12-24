using System;
using System.Linq;
using System.Threading.Tasks;
using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppApi.DataAccess.Repositories.AuthService
{
    public class RoleRepository : GenericRepository<Role>, ICommonRepository
    {
        public RoleRepository(ApplicationDbContext context, ILogger<Role> logger) : base(context, logger)
        {

        }

        public override async Task<Role> UpsertAsync(Role entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                existItem.Name = entity.Name;
                existItem.Description = entity.Description;
                // existItem.RoleCode = entity.RoleCode;

                existItem.UpdatedBy = entity.UpdatedBy;
                existItem.UpdatedDate = DateTime.UtcNow;

                return existItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(RoleRepository));
                throw ex;
            }
        }
    }
}