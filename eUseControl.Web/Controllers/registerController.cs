using System;
using System.Data.Entity;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Register
        public ActionResult Index()
        {
            return View(new User());
        }

        // GET: Register/ViewDatabase
        [AllowAnonymous]
        public ActionResult ViewDatabase()
        {
            try
            {
                // Ensure database exists and is up to date
                if (!_context.Database.Exists())
                {
                    _context.Database.Create();
                }

                // Try to access users to verify database is working
                var users = _context.Users.ToList();
                
                // Add statistics
                ViewBag.TotalUsers = users.Count;
                ViewBag.NewUsersToday = users.Count(u => u.CreatedAt.Date == DateTime.Today);
                ViewBag.DatabaseName = _context.Database.Connection.Database;
                ViewBag.LastUpdated = DateTime.Now;

                return View(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ViewDatabase: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Debug.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }

                ViewBag.ErrorMessage = "An error occurred while accessing the database. Please try again later.";
                return View(new List<User>());
            }
        }

        [HttpGet]
        public ActionResult TestDatabase()
        {
            try
            {
                bool canConnect = _context.Database.Exists();
                if (!canConnect)
                {
                    ViewBag.Success = false;
                    ViewBag.Message = "Database does not exist.";
                    return View();
                }

                int userCount = _context.Users.Count();
                ViewBag.Success = true;
                ViewBag.Message = $"Database connection successful. User count: {userCount}";
                ViewBag.DatabaseName = _context.Database.Connection.Database;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.Message = $"Database error: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if email already exists
                    if (_context.Users.Any(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View("Index", user);
                    }

                    // Set creation date
                    user.CreatedAt = DateTime.Now;

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Registration error: {ex.Message}");
                
                // Add a user-friendly error message
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
            }

            return View("Index", user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            return input.Trim()
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}   