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
        public DbSet<ProcessEntity> Process { get; set; }
        public DbSet<StateEntity> State { get; set; }
        public DbSet<HolderEntity> Holder { get; set; }
        public DbSet<StateHistoryEntity> StateHistory { get; set; }
        public DbSet<ApplicationEntity> Application { get; set; }
        public DbSet<HolderHistoryEntity> HolderHistory { get; set; }
        public DbSet<DocumentEntity> Document { get; set; }
        public DbSet<DocumentHistoryEntity> DocumentHistory { get; set; }
        public DbSet<AssociateEntity> Associate { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }
    } }
