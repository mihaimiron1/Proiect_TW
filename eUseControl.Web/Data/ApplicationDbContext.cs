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
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<TransferCard> TransferCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
} 