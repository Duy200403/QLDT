using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace AppApi.DataAccess.Base
{
    public delegate object ServiceFactory(Type serviceType);

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        private static readonly List<PropertyInfo> _properties;
        private readonly ServiceFactory _serviceFactory;

        static UnitOfWork()
        {
            _properties = typeof(UnitOfWork).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(t => typeof(IGenericRepository).IsAssignableFrom(t.PropertyType) && t.PropertyType.IsGenericType)
                .ToList();
        }

        public UnitOfWork(ApplicationDbContext context, ServiceFactory serviceFactory, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            foreach (var prop in _properties)
            {
                prop.SetValue(this, serviceFactory(prop.PropertyType));
            }
            _serviceFactory = serviceFactory;
        }

        //public UnitOfWork(ApplicationDbContext context, ServiceFactory serviceFactory, ILoggerFactory loggerFactory)
        //{
        //    _context = context;
        //    _logger = loggerFactory.CreateLogger("logs");
        //    _serviceFactory = serviceFactory;

        //    // 🔹 PHẦN THAY ĐỔI: chỉ init các repository tương ứng với DbSet trong context
        //    var validRepos = _context.GetType()
        //        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //        .Where(p =>
        //            p.PropertyType.IsGenericType &&
        //            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
        //        .Select(p =>
        //            typeof(IGenericRepository<>)
        //                .MakeGenericType(p.PropertyType.GetGenericArguments()[0]))
        //        .ToHashSet();

        //    foreach (var prop in _properties)
        //    {
        //        if (validRepos.Contains(prop.PropertyType))
        //        {
        //            prop.SetValue(this, serviceFactory(prop.PropertyType));
        //        }
        //    }
        //}


        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return (IGenericRepository<T>)_serviceFactory(typeof(IGenericRepository<T>));
        }

        public IGenericRepository<FileManager> FileManager { get; private set; }
        public IGenericRepository<EmailConfig> EmailConfig { get; private set; }
        public IGenericRepository<Log> Log { get; private set; }
        public IGenericRepository<Account> Account { get; private set; }
        public IGenericRepository<LoginHistory> LoginHistory { get; private set; }
        public IGenericRepository<Role> Role { get; private set; }
        public IGenericRepository<ApiRoleMapping> ApiRoleMapping { get; private set; }
        public IGenericRepository<MenuItem> MenuItem { get; private set; }
        public IGenericRepository<Test> Test { get; private set; }
        public IGenericRepository<GoiThauKeHoach> GoiThauKeHoach { get; private set; }
        // public IGenericRepository<AccountPatientInfo> AccountPatientInfo { get; private set; }
    }
}