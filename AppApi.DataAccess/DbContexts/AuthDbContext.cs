using Microsoft.EntityFrameworkCore;
using AppApi.Entities.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace AppApi.DataAccess.Base
{
    /// <summary>
    /// DbContext for the Authentication service, managing only Account and LoginHistory.
    /// </summary>
    public class AuthDbContext : ApplicationDbContext, IDataProtectionKeyContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<LoginHistory> LoginHistory { get; set; }
        public virtual DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // concurrent write 
            modelBuilder.Entity<Account>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<Log>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<FileManager>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<LoginHistory>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<Role>().Property(c => c.Timestamp).IsRowVersion();
            // modelBuilder.Entity<EmailConfig>().Property(c => c.Timestamp).IsRowVersion();

            modelBuilder.Entity<Account>()
                .HasQueryFilter(ap => !ap.IsDeleted);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(ap => !ap.IsDeleted);
        }
    }
}