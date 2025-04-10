using System.Data.Entity.Migrations;

namespace eUseControl.Web.Data
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // This method will be called after migrating to the latest version.
            // You can use this method to seed data into the database.
        }
    }
} 