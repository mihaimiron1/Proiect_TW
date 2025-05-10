using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using eUseControl.Web.Models;

namespace eUseControl.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<TransferCard> TransferCards { get; set; }
        public DbSet<SupportTable> SupportTable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            // Configure TransferCard entity
            modelBuilder.Entity<TransferCard>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TransferCard>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
} 