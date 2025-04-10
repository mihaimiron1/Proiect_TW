using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Data.Entity;
using eUseControl.Web.Data;
using System.Diagnostics;

namespace eUseControl.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                // First, try to delete the existing database
                using (var context = new ApplicationDbContext())
                {
                    if (context.Database.Exists())
                    {
                        context.Database.Delete();
                        Debug.WriteLine("Existing database deleted");
                    }
                }

                // Now create a fresh database
                Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());

                using (var context = new ApplicationDbContext())
                {
                    context.Database.Create();
                    Debug.WriteLine("New database created successfully");
                }

                // Finally, set up migrations for future updates
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
                throw new Exception("Failed to initialize database. Please ensure SQL Server LocalDB is installed and running.", ex);
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}