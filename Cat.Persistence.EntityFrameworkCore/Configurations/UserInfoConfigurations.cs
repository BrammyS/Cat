using Cat.Persistence.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cat.Persistence.EntityFrameworkCore.Configurations
{
    public class UserInfoConfigurations : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfos");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("numeric(20,0)").HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.ServerId).HasColumnType("numeric(20,0)").HasColumnName("ServerId").IsRequired();
            builder.Property(x => x.UserId).HasColumnType("numeric(20,0)").HasColumnName("UserId").IsRequired();
            builder.Property(x => x.Xp).HasColumnType("numeric(20,0)").HasColumnName("Xp").IsRequired();
            builder.Property(x => x.TimeConnected).HasColumnType("numeric(20,0)").HasColumnName("TimeConnected").IsRequired();
            builder.Property(x => x.Level).HasColumnType("numeric(20,0)").HasColumnName("Level").IsRequired();
            builder.Property(x => x.MessagesSend).HasColumnType("numeric(20,0)").HasColumnName("MessagesSend").IsRequired();
            builder.Property(x => x.LastMessageSend).HasColumnType("datetime").HasColumnName("LastMessageSend").IsRequired();
            builder.Property(x => x.LastVoiceStateUpdateReceived).HasColumnType("datetime").HasColumnName("LastVoiceStateUpdateReceived").IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.UserInfos)
                .HasPrincipalKey(x=>x.Id).HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull).IsRequired();

            builder.HasOne(x => x.Server).WithMany(x => x.UserInfos)
                .HasPrincipalKey(x=>x.Id).HasForeignKey(x=>x.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull).IsRequired();
        }
    }
}