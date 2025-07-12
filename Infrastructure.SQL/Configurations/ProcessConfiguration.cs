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
    class ProcessConfiguration : IEntityTypeConfiguration<ProcessEntity>
    {
        public void Configure(EntityTypeBuilder<ProcessEntity> builder)
        {
            builder.ToTable("Process", "dbo");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.HasIndex(p => p.ProcessCode)
                   .IsUnique();

            builder.Property(p => p.ProcessCode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.Property(p => p.Notes)
                   .HasMaxLength(50);

            builder.HasOne(p => p.State)
                  .WithMany()
                  .HasForeignKey(p => p.StateId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CreatedBy)
                   .WithMany()
                   .HasForeignKey(p => p.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Application)
                   .WithMany()
                   .HasForeignKey(p => p.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Holder)
                   .WithMany()
                   .HasForeignKey(p => p.HolderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Documents)
                   .WithOne(d => d.Process)
                   .HasForeignKey(d => d.ProcessId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
