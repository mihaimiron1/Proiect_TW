using System.Data.Entity;
using eUseControl.Domain.Entities.User;

namespace eUseControl.Domain.Data
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ULoginData> LoginHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
} 