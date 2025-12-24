using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.Extensions.Logging;

namespace AppApi.DataAccess.Repositories.WebApi
{
    public class FileManagerRepository : GenericRepository<FileManager>, ICommonRepository
    {
        public FileManagerRepository(ApplicationDbContext context, ILogger<FileManager> logger) : base(context, logger)
        {

        }
    }
}