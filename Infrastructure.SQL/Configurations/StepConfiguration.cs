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
    class StepConfiguration : IEntityTypeConfiguration<StepEntity>
    {
        public void Configure(EntityTypeBuilder<StepEntity> builder)
        {

            builder.ToTable("Step", "dbo");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Description).HasMaxLength(100);
            builder.Property(a => a.Order).IsRequired();
            builder.Property(a => a.IsFinal).IsRequired();
            builder.HasOne(a => a.State)
                  .WithMany() 
                  .HasForeignKey(a => a.StateId)
                  .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(a => a.Holder)
                   .WithMany() 
                   .HasForeignKey(a => a.HolderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Executions)
                  .WithOne(e => e.Step)
                  .HasForeignKey(e => e.StepId)
                  .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
