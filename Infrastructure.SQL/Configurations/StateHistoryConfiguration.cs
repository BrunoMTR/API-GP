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
    class StateHistoryConfiguration : IEntityTypeConfiguration<StateHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<StateHistoryEntity> builder)
        {
            builder.ToTable("StateHistory", "dbo");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.StartedAt).IsRequired();
            builder.HasOne(h => h.Process)
                        .WithMany()
                        .HasForeignKey(h => h.ProcessId);
            builder.HasOne(h => h.State)
                        .WithMany()
                        .HasForeignKey(h => h.StateId);
            builder.HasOne(h => h.ChangedBy)
                        .WithMany()
                        .HasForeignKey(h => h.ChangedById);
        }
    }
}
