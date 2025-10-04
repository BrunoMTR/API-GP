using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SQL.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            // Nome da tabela
            builder.ToTable("History");

            // Chave primária
            builder.HasKey(h => h.Id);

            // Propriedades obrigatórias
            builder.Property(h => h.UpdatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(h => h.Note)
                .HasMaxLength(200);

            builder.Property(h => h.Notified)
                   .IsRequired();

            builder.Property(h => h.UpdatedAt)
                   .IsRequired();

            // Relacionamento com Application
            builder.HasOne<ApplicationEntity>()
                   .WithMany() // histórico não precisa navegar para Application
                   .HasForeignKey(h => h.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Process
            builder.HasOne<ProcessEntity>()
                   .WithMany() // histórico não precisa navegar para Process
                   .HasForeignKey(h => h.ProcessId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Unit (campo At)
            builder.HasOne<UnitEntity>()
                   .WithMany() // histórico não precisa navegar para Unit
                   .HasForeignKey(h => h.At)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
