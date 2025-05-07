using System.Data.Entity;
using eUseControl.Web.Models;
using System.Linq;

namespace eUseControl.Web.Data
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Add a test user if the Users table is empty
            if (!context.Users.Any())
            {
                context.Users.Add(new User 
                { 
                    Name = "Test User",
                    Email = "test@example.com",
                    Password = "password123", // In real application, this should be hashed
                    PhoneNumber = "1234567890",
                    City = "Test City",
                    Country = "Test Country"
                });
                
                context.SaveChanges();
            }
            
            base.Seed(context);
        }
    }
}