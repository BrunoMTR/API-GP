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
    class NodeConfiguration : IEntityTypeConfiguration<NodeEntity>
    {
        public void Configure(EntityTypeBuilder<NodeEntity> builder)
        {
            builder.ToTable("Node");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Direction)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(g => g.Approvals)
                .IsRequired(false);

            builder.HasOne(g => g.Application)
                .WithMany() 
                .HasForeignKey(g => g.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.Origin)
                .WithMany()  
                .HasForeignKey(g => g.OriginId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.Destination)
                .WithMany()  
                .HasForeignKey(g => g.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
