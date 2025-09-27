using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notification", "dbo");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Body).IsRequired().HasMaxLength(500);
            builder.Property(n => n.Subject).IsRequired().HasMaxLength(50);
            builder.Property(n => n.Bcc).HasMaxLength(50);
            builder.Property(n => n.Cc).HasMaxLength(50);
            builder.Property(n => n.At).HasMaxLength(50);
            builder.Property(n => n.ProcessId).IsRequired();

            builder.HasOne<ProcessEntity>()
                 .WithMany() 
                 .HasForeignKey(n => n.ProcessId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
