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
    class StepHistoryConfiguration : IEntityTypeConfiguration<StepHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<StepHistoryEntity> builder)
        {
            builder.ToTable("StepHistory", "dbo");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.StartedAt).IsRequired();
            builder.Property(s => s.CompletedAt);
            builder.HasOne(s => s.Process)
                    .WithMany(p => p.Executions)
                    .HasForeignKey(s => s.ProcessId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(s => s.Step)
                   .WithMany() 
                   .HasForeignKey(s => s.StepId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.ExecutedBy)
                   .WithMany() 
                   .HasForeignKey(s => s.ExecutedById)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
