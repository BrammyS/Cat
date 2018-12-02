using Cat.Persistence.Domain.Tables;
using Cat.Persistence.EntityFrameworkCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cat.Persistence.EntityFrameworkCore.Models
{
    public class CatContext : DbContext
    {
        public CatContext()
        {
        }

        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        // Unable to generate entity type for table 'dbo.Users'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(DatabaseConfig.Data.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServerConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new UserInfoConfigurations());
        }
    }
}
