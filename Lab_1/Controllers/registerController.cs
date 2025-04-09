using System;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Data;
using System.Linq;

namespace eUseControl.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register/ViewDatabase
        public ActionResult ViewDatabase()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult TestDatabase()
        {
            try
            {
                // Test database connection
                bool canConnect = _context.Database.Exists();
                
                // Try to access the Users table
                int userCount = _context.Users.Count();
                
                ViewBag.Success = true;
                ViewBag.Message = $"Database connection successful. User count: {userCount}";
                return View("TestDatabase");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.Message = $"Database error: {ex.Message}";
                return View("TestDatabase");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email already exists
                    if (_context.Users.Any(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exists");
                        return View("Index", model);
                    }

                    // Set creation date
                    model.CreatedAt = DateTime.Now;

                    // Add user to database
                    _context.Users.Add(model);
                    _context.SaveChanges();

                    // Redirect to success page or login
                    TempData["SuccessMessage"] = "Registration successful! You can now login.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while registering. Please try again.");
                    // Log the error - in production you should use proper logging
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Index", model);
        }
    }
}