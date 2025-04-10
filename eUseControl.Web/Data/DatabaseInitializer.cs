using System.Data.Entity;
using eUseControl.Web.Models;

namespace eUseControl.Web.Data
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // You can add initial/test data here if needed
            /*
            context.Users.Add(new User 
            { 
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123" // In real application, this should be hashed
            });
            */
            
            base.Seed(context);
        }
    }
}