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
    public class UserConfigurations : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User", "dbo");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(10);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(10);
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.Role).HasMaxLength(50);
            builder.Property(u => u.LastChanged);
        }
    }
}
