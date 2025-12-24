using Microsoft.EntityFrameworkCore;
//using ApiWebsite.Models;
using System;
//using ApiWebsite.Helper;
//using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.IO;
using AppApi.Entities.Models;
using System.Reflection;

namespace AppApi.DataAccess.Base
{
    public abstract class ApplicationDbContext : DbContext
    {
        // the dbset property will tell ef core that we have
        // a table that needs to be created if doesnt exist
        // public virtual DbSet<Account> Account { get; set; }
         public virtual DbSet<Log> Log { get; set; }
         public virtual DbSet<ApiRoleMapping> ApiRoleMapping { get; set; }
         public virtual DbSet<MenuItem> MenuItem { get; set; }
        // public virtual DbSet<FileManager> FileManager { get; set; }
        // public virtual DbSet<EmailConfig> EmailConfig { get; set; }
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // concurrent write 
            // modelBuilder.Entity<Account>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<Log>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<ApiRoleMapping>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<MenuItem>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<FileManager>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<LoginHistory>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<EmailConfig>().Property(c => c.Timestamp).IsRowVersion();

            modelBuilder.Entity<ApiRoleMapping>()
                .HasQueryFilter(ap => !ap.IsDeleted);

            modelBuilder.Entity<MenuItem>()
                .HasQueryFilter(ap => !ap.IsDeleted);

            // Áp dụng IsRowVersion cho tất cả các property có tên "Timestamp"
            // foreach (var entity in modelBuilder.Model.GetEntityTypes())
            // {
            //     var prop = entity.ClrType.GetProperty("Timestamp");
            //     if (prop != null && prop.PropertyType == typeof(byte[]))
            //     {
            //         modelBuilder.Entity(entity.ClrType)
            //                .Property("Timestamp")
            //                .IsRowVersion();
            //     }
            // }

            // // Áp dụng soft‐delete global filter cho mọi entity có IsDeleted
            // foreach (var entity in modelBuilder.Model.GetEntityTypes())
            // {
            //     var prop = entity.ClrType.GetProperty("IsDeleted");
            //     if (prop != null && prop.PropertyType == typeof(bool))
            //     {
            //         var method = typeof(ApplicationDbContext)
            //             .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
            //             .MakeGenericMethod(entity.ClrType);
            //         method.Invoke(null, new object[] { modelBuilder });
            //     }
            // }

            // modelBuilder.UseOpenIddict();
        }

        // Helper generic để apply HasQueryFilter(e => !e.IsDeleted)
        // private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder)
        //     where TEntity : class
        // {
        //     builder.Entity<TEntity>()
        //            .HasQueryFilter(
        //              e => EF.Property<bool>(e, "IsDeleted") == false
        //            );
        // }
    }
}