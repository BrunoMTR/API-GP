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
    class HolderConfiguration : IEntityTypeConfiguration<HolderEntity>
    {
        public void Configure(EntityTypeBuilder<HolderEntity> builder)
        {
            builder.ToTable("Holder", "dbo");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Acronym).IsRequired().HasMaxLength(20);
            builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
            builder.Property(h => h.Email).IsRequired().HasMaxLength(100);
        }
    }
}
