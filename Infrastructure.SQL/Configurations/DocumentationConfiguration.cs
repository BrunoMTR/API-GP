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
    public class DocumentationConfiguration : IEntityTypeConfiguration<DocumentationEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentationEntity> builder)
        {
            builder.ToTable("Documentation", "dbo");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FileName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.FilePath).IsRequired().HasMaxLength(200);
            builder.Property(a => a.FileType).IsRequired().HasMaxLength(50);
            builder.Property(a => a.UploadedBy).IsRequired().HasMaxLength(50);
            builder.Property(a => a.UploadedAt).IsRequired().HasDefaultValueSql("GETDATE()"); ;
            builder.Property(a => a.At).IsRequired();
            builder.HasIndex(a => a.FileName).IsUnique();
            
        }
    }
}
