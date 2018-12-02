using Cat.Persistence.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cat.Persistence.EntityFrameworkCore.Configurations
{
    public class ServerConfigurations : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("Servers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("numeric(20,0)").HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(MAX)").HasColumnName("Name").IsRequired();
            builder.Property(x => x.JoinDate).HasColumnType("date").HasColumnName("JoinDate").IsRequired();
            builder.Property(x => x.Active).HasColumnType("bit").HasColumnName("Active").IsRequired();
            builder.Property(x => x.TotalMembers).HasColumnType("numeric(20,0)").HasColumnName("TotalMembers").IsRequired();
            builder.Property(x => x.LevelUpChannel).HasColumnType("numeric(20,0)").HasColumnName("LevelUpChannel").IsRequired(false);
            builder.Property(x => x.Prefix).HasColumnType("nvarchar(MAX)").HasColumnName("Prefix").IsRequired(false);

            builder.HasMany(x => x.UserInfos).WithOne(x => x.Server)
                .HasPrincipalKey(x => x.Id).HasForeignKey(x => x.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull).IsRequired();
        }
    }
}
