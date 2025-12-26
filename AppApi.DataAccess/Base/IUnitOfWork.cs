using System.Threading.Tasks;
using AppApi.Entities.Models;

namespace AppApi.DataAccess.Base
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        IGenericRepository<T> GetRepository<T>() where T : class;
        IGenericRepository<FileManager> FileManager { get; }
        IGenericRepository<EmailConfig> EmailConfig { get; }
        IGenericRepository<Account> Account { get; }
        IGenericRepository<Log> Log { get; }
        IGenericRepository<LoginHistory> LoginHistory { get; }
        IGenericRepository<Role> Role { get; }
        IGenericRepository<ApiRoleMapping> ApiRoleMapping { get; }
        IGenericRepository<MenuItem> MenuItem { get; }
        // IGenericRepository<AccountPatientInfo> AccountPatientInfo { get; }
        IGenericRepository<GoiThauKeHoach> GoiThauKeHoach { get; }
    }
}