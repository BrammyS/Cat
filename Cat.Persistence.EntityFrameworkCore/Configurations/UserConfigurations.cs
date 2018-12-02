using Cat.Persistence.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cat.Persistence.EntityFrameworkCore.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("numeric(20,0)").HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(MAX)").HasColumnName("Name").IsRequired();
            builder.Property(x => x.SpamWarning).HasColumnType("numeric(20,0)").HasColumnName("SpamWarning").IsRequired();
            builder.Property(x => x.TotalTimesTimedOut).HasColumnType("numeric(20,0)").HasColumnName("TimesTimedOut").IsRequired();
            builder.Property(x => x.CommandUsed).HasColumnType("datetime").HasColumnName("CommandUsed").IsRequired();

            builder.HasMany(x => x.UserInfos).WithOne(x => x.User)
                .HasPrincipalKey(x=>x.Id).HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
