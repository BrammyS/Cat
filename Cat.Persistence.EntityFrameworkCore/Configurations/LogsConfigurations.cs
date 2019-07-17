using Cat.Persistence.Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cat.Persistence.EntityFrameworkCore.Configurations
{
    public class LogsConfigurations : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");
            builder.HasKey(x => x.LogId);

            builder.Property(x => x.LogId).HasColumnType("numeric(20,0)").HasColumnName("LogId").IsRequired();
            builder.Property(x => x.MessageId).HasColumnType("numeric(20,0)").HasColumnName("MessageId").IsRequired();
        }
    }
}
