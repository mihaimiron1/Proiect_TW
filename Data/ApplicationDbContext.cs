using Microsoft.EntityFrameworkCore;
using Proiect_TW.Models;

namespace Proiect_TW.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TransferCard> TransferCards { get; set; }
    }
} 