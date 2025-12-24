using System;
using System.Linq;
using System.Threading.Tasks;
using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppApi.DataAccess.Repositories.WebApi
{
    public class TestRepository : GenericRepository<Test>, ICommonRepository
    {
        public TestRepository(ApplicationDbContext context, ILogger<Test> logger) : base(context, logger)
        {

        }

        public override async Task<Test> UpsertAsync(Test entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                existItem.TestName = entity.TestName;
                existItem.TestCode = entity.TestCode;

                existItem.UpdatedBy = entity.UpdatedBy;
                existItem.UpdatedDate = DateTime.UtcNow;

                return existItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(EmailConfigRepository));
                throw ex;
            }
        }
    }
}