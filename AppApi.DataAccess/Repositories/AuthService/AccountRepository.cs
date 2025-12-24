using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.Extensions.Logging;

namespace AppApi.DataAccess.Repositories.AuthService
{
    public class AccountRepository : GenericRepository<Account>, ICommonRepository
    {
        public AccountRepository(ApplicationDbContext context, ILogger<Account> logger) : base(context, logger)
        {

        }

    }
}