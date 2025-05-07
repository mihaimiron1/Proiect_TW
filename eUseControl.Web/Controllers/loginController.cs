using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net;
using System.Web.Security;
using System.Web;
using eUseControl.Helpers;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController()
        {
            _context = new ApplicationDbContext();
            EnsureAdminExists();
        }

        private void EnsureAdminExists()
        {
            try
            {
                if (!_context.Database.Exists())
                {
                    _context.Database.Create();
                }

                // Check if admin exists
                var adminUser = _context.Users.FirstOrDefault(u => u.Email == "admin@admin");
                if (adminUser == null)
                {
                    // Create admin user with hashed password
                    adminUser = new User
                    {
                        Email = "admin@admin",
                        Password = PasswordHasher.HashPassword("admin1"),
                        Name = "Administrator",
                        Role = "Admin",
                        CreatedAt = DateTime.Now
                    };
                    _context.Users.Add(adminUser);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ensuring admin exists: " + ex.ToString());
            }
        }

        private string GetClientIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }
            
            // If still empty or localhost, try to get the actual IP
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        ipAddress = client.DownloadString("https://api.ipify.org");
                    }
                }
                catch
                {
                    ipAddress = "Unknown";
                }
            }

            return ipAddress;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Check if user is already authenticated via cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = Request.Cookies["UserAuth"];
                try
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        // User is already authenticated, redirect to appropriate page
                        if (ticket.UserData == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (ArgumentException)
                {
                    // Invalid cookie, clear it
                    var expiredCookie = new HttpCookie("UserAuth", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(expiredCookie);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter both email and password.");
                return View();
            }

            try
            {
                // Check if user exists in the database
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                // Create a new login record
                var loginRecord = new LoginRecord
                {
                    Email = email,
                    LoginTime = DateTime.Now,
                    IPAddress = GetClientIPAddress(),
                    Success = false
                };

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    _context.LoginRecords.Add(loginRecord);
                    _context.SaveChanges();
                    return View();
                }

                // Verify the password using the hashed version
                if (!PasswordHasher.VerifyPassword(password, user.Password))
                {
                    ModelState.AddModelError("", "Password incorrect.");
                    _context.LoginRecords.Add(loginRecord);
                    _context.SaveChanges();
                    return View();
                }

                // Login successful
                loginRecord.Success = true;
                _context.LoginRecords.Add(loginRecord);
                _context.SaveChanges();

                // Create authentication ticket
                var authTicket = new FormsAuthenticationTicket(
                    1,                              // Version
                    user.Email,                     // User name
                    DateTime.Now,                   // Issue time
                    DateTime.Now.AddDays(7),        // Expiration time (7 days)
                    true,                          // Persistent
                    user.Role                      // User data (role)
                );

                // Encrypt the ticket
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                // Create the cookie
                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection,
                    Expires = DateTime.Now.AddDays(7)
                };

                // Add the cookie to the response
                Response.Cookies.Add(authCookie);

                // Set session variables
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Name;
                Session["UserRole"] = user.Role;

                // If user is admin, redirect to admin dashboard
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // Check if there's a return URL for non-admin users
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Login Error: " + ex.ToString());
                return View();
            }
        }

        public ActionResult Logout()
        {
            // Sign out from forms authentication
            FormsAuthentication.SignOut();

            // Clear the authentication cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = new HttpCookie("UserAuth", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(authCookie);
            }

            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Redirect to home page
            return RedirectToAction("Index", "Home");
        }

        // GET: Login/ViewLoginHistory
        public ActionResult ViewLoginHistory()
        {
            // Check if user is admin
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Ensure database exists
                if (!_context.Database.Exists())
                {
                    _context.Database.Create();
                }

                // Check if LoginRecords table exists by trying to access it
                try
                {
                    var loginRecords = _context.LoginRecords
                        .OrderByDescending(l => l.LoginTime)
                        .ToList();

                    ViewBag.LastUpdated = DateTime.Now;
                    return View(loginRecords);
                }
                catch (Exception ex)
                {
                    // If there's an error accessing the table, it might not exist
                    ViewBag.ErrorMessage = "The login history table has not been initialized yet. Please try logging in first.";
                    ViewBag.DetailedError = ex.Message;
                    return View(new System.Collections.Generic.List<LoginRecord>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while accessing the login history.";
                ViewBag.DetailedError = ex.Message;
                return View(new System.Collections.Generic.List<LoginRecord>());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}