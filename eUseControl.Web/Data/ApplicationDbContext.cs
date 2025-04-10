using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using eUseControl.Web.Models;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace eUseControl.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Enable detailed error messages
            Database.Log = s => Debug.WriteLine(s);
            
            // Disable lazy loading for better control
            Configuration.LazyLoadingEnabled = false;
            
            // Enable automatic detection of changes
            Configuration.AutoDetectChangesEnabled = true;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                
                // Configure User table
                modelBuilder.Entity<User>()
                    .ToTable("Users")
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
                    .HasMaxLength(20)
                    .IsOptional();

                modelBuilder.Entity<User>()
                    .Property(u => u.City)
                    .HasMaxLength(100)
                    .IsOptional();

                modelBuilder.Entity<User>()
                    .Property(u => u.Country)
                    .HasMaxLength(100)
                    .IsOptional();

                modelBuilder.Entity<User>()
                    .Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

                // Configure LoginRecord table
                modelBuilder.Entity<LoginRecord>()
                    .ToTable("LoginRecords")
                    .HasKey(l => l.Id);

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.Id)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.LoginTime)
                    .IsRequired()
                    .HasColumnType("datetime2");

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.Success)
                    .IsRequired();

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.IPAddress)
                    .HasMaxLength(45)
                    .IsOptional();

                modelBuilder.Entity<LoginRecord>()
                    .Property(l => l.UserAgent)
                    .HasMaxLength(500)
                    .IsOptional();

                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnModelCreating: {ex.Message}");
                throw new Exception("Failed to create database model. See inner exception for details.", ex);
            }
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
                        var message = $"Entity: {validationErrors.Entry.Entity.GetType().Name} Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}";
                        Debug.WriteLine(message);
                        errorMessages.AppendLine(message);
                    }
                }
                throw new Exception($"Validation failed: {errorMessages}", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SaveChanges: {ex.Message}");
                throw new Exception("Failed to save changes to database. See inner exception for details.", ex);
            }
        }
    }
} 