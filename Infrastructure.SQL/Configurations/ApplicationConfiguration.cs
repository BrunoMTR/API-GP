using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Configurations
{
    class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.ToTable("Application", "dbo");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Abbreviation).IsRequired().HasMaxLength(10);
            builder.Property(a => a.Team).IsRequired().HasMaxLength(50);
            builder.Property(a => a.TeamEmail).HasMaxLength(50);
            builder.Property(a => a.ApplicationEmail).HasMaxLength(50);

            builder.HasMany(a => a.Processes)
                  .WithOne(p => p.Application)
                  .HasForeignKey(p => p.ApplicationId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Steps)
                   .WithOne(s => s.Application)
                   .HasForeignKey(s => s.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.Name).IsUnique();
            builder.HasIndex(a => a.Abbreviation).IsUnique();
        }
    }
}
