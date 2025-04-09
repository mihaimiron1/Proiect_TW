using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using eUseControl.Web.Models;

namespace eUseControl.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            // This will create the database if it doesn't exist
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public ApplicationDbContext() : base("DefaultConnection")
        {
            // This ensures the database is created
            Database.CreateIfNotExists();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            // Configure User table
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
} 