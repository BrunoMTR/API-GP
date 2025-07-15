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
    class DocumentHistoryConfiguration : IEntityTypeConfiguration<DocumentHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentHistoryEntity> builder)
        {
            builder.ToTable("DocumentHistory", "dbo");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.ActionType).IsRequired().HasMaxLength(20);
            builder.Property(h => h.ActionDate).IsRequired();
            builder.Property(h => h.Notes)
                .HasMaxLength(500);
            builder.HasOne(h => h.Document)
               .WithMany() 
               .HasForeignKey(h => h.DocumentId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.PerformedBy)
             .WithMany() 
             .HasForeignKey(h => h.PerformedById)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Step)
                .WithMany() 
                .HasForeignKey(h => h.StepId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
