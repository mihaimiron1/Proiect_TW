using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using eUseControl.Web.Models;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            // Create database if it doesn't exist and enable automatic migrations
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Enable lazy loading
            this.Configuration.LazyLoadingEnabled = true;
            
            // Enable automatic detection of changes
            this.Configuration.AutoDetectChangesEnabled = true;

            // Disable proxy creation for better performance when not needed
            this.Configuration.ProxyCreationEnabled = false;

            // This will create the database if it doesn't exist
            Database.CreateIfNotExists();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            // Configure User table
            modelBuilder.Entity<User>()
                .ToTable("Users"); // Explicitly set table name

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            // Create unique index for email
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.City)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Country)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessages.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw new Exception(errorMessages.ToString(), ex);
            }
        }
    }
} 