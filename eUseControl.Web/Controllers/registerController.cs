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
using eUseControl.Helpers;
using System.ComponentModel.DataAnnotations;

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
                    Debug.WriteLine("Database created successfully");
                }

                // Try to access users to verify database is working
                var users = _context.Users.ToList();
                
                // Enhanced statistics
                var now = DateTime.Now;
                ViewBag.TotalUsers = users.Count;
                ViewBag.NewUsersToday = users.Count(u => u.CreatedAt.Date == DateTime.Today);
                ViewBag.UsersThisWeek = users.Count(u => u.CreatedAt >= now.AddDays(-7));
                ViewBag.UsersThisMonth = users.Count(u => u.CreatedAt >= now.AddMonths(-1));
                ViewBag.DatabaseName = _context.Database.Connection.Database;
                ViewBag.LastUpdated = now;
                ViewBag.MostRecentUser = users.OrderByDescending(u => u.CreatedAt).FirstOrDefault()?.Name ?? "None";

                Debug.WriteLine($"Successfully retrieved {users.Count} users from database");
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

                var errorMessage = ex.InnerException != null 
                    ? $"Database error: {ex.Message}. Details: {ex.InnerException.Message}"
                    : $"Database error: {ex.Message}";

                ViewBag.ErrorMessage = "An error occurred while accessing the database. Please try again later.";
                ViewBag.DetailedError = errorMessage;
                ViewBag.LastUpdated = DateTime.Now;
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

                    // Hash the password before saving
                    user.Password = PasswordHasher.HashPassword(user.Password);

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Auto-login after registration
                    var authTicket = new System.Web.Security.FormsAuthenticationTicket(
                        1,
                        user.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(7),
                        true,
                        user.Role
                    );
                    string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new System.Web.HttpCookie("UserAuth", encryptedTicket)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection,
                        Expires = DateTime.Now.AddDays(7)
                    };
                    Response.Cookies.Add(authCookie);
                    Session["UserId"] = user.Id;
                    Session["UserEmail"] = user.Email;
                    Session["UserName"] = user.Name;
                    Session["UserRole"] = user.Role;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || user.Email == "admin@admin")
            {
                TempData["ErrorMessage"] = "Cannot delete this user.";
                return RedirectToAction("ViewDatabase");
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("ViewDatabase");
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