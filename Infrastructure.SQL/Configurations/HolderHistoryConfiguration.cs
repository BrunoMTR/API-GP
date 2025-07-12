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
    class HolderHistoryConfiguration : IEntityTypeConfiguration<HolderHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HolderHistoryEntity> builder)
        {
            builder.ToTable("HolderHistory", "dbo");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.MovedAt).IsRequired();
            builder.HasOne(h => h.Process)
                         .WithMany()
                         .HasForeignKey(h => h.ProcessId);
            builder.HasOne(h => h.Holder)
                         .WithMany()
                         .HasForeignKey(h => h.HolderId);
            builder.HasOne(h => h.ChangedBy)
                         .WithMany()
                         .HasForeignKey(h => h.ChangedById);
        }
    }
}
