using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;

namespace AppApi.DataAccess.Repositories.LogServ
{
    public class LogRepository : GenericRepository<Log>, ICommonRepository
    {
        public LogRepository(ApplicationDbContext context, ILogger<Log> logger) : base(context, logger)
        {
        }
    }
}