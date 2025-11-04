using Infrastructure.SQL.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.DB
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UnitEntity> Unit { get; set; }
 
        public DbSet<ApplicationEntity> Application { get; set; }
        public DbSet<NodeEntity> Node { get; set; }
        public DbSet<ProcessEntity> Process { get; set; }
        public DbSet<HistoryEntity> History { get; set; }
        public DbSet<DocumentationEntity> Documentation { get; set; }
        public DbSet<NotificationEntity> Notification { get; set; }
        public DbSet<UserEntity>User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }
    } }
