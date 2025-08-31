using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SQL.Configurations
{
    public class ProcessConfiguration : IEntityTypeConfiguration<ProcessEntity>
    {
        public void Configure(EntityTypeBuilder<ProcessEntity> builder)
        {
            builder.ToTable("Process", "dbo");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ApplicationId)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.At)
                   .IsRequired();

            builder.Property(p => p.Approvals)
                   .IsRequired();

            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasDefaultValue(Domain.DTOs.ProcessStatus.Initiated);

            // Relacionamento com Application
            builder.HasOne<ApplicationEntity>()
                   .WithMany()
                   .HasForeignKey(p => p.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Unit (At)
            builder.HasOne<UnitEntity>()
                   .WithMany()
                   .HasForeignKey(p => p.At)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.ApplicationId);

            builder.HasMany(p => p.Histories)
                    .WithOne()
                    .HasForeignKey(h => h.ProcessId)
                    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
