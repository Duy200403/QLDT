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
    public class EmailConfigRepository : GenericRepository<EmailConfig>, ICommonRepository
    {
        public EmailConfigRepository(ApplicationDbContext context, ILogger<EmailConfig> logger) : base(context, logger)
        {

        }

        public override async Task<EmailConfig> UpsertAsync(EmailConfig entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                existItem.Email = entity.Email;
                existItem.Password = entity.Password;
                existItem.IsActive = entity.IsActive;
                existItem.MailServer = entity.MailServer;
                existItem.Port = entity.Port;
                existItem.EnableSSl = entity.EnableSSl;
                existItem.EmailTitle = entity.EmailTitle;

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