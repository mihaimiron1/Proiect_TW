using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController()
        {
            _context = new ApplicationDbContext();
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

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                // Check if database exists and create if it doesn't
                if (!_context.Database.Exists())
                {
                    _context.Database.Create();
                }

                string email = form["email"];
                string password = form["password"];

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Email and password are required.");
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
                        // User doesn't exist - redirect to register
                        TempData["Message"] = "User not found. Please register first.";
                        _context.LoginRecords.Add(loginRecord);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Register");
                    }

                    if (user.Password != password) // Note: In production, use proper password hashing
                    {
                        ModelState.AddModelError("", "Invalid password.");
                        _context.LoginRecords.Add(loginRecord);
                        _context.SaveChanges();
                        return View();
                    }

                    // Login successful
                    loginRecord.Success = true;
                    _context.LoginRecords.Add(loginRecord);
                    _context.SaveChanges();

                    // Set session variables
                    Session["UserId"] = user.Id;
                    Session["UserEmail"] = user.Email;
                    Session["UserName"] = user.Name;
                    
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateException dbEx)
                {
                    ModelState.AddModelError("", "Database error: " + dbEx.InnerException?.Message ?? dbEx.Message);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Database connection error: " + ex.Message);
                // Log the full error details
                System.Diagnostics.Debug.WriteLine("Login Error: " + ex.ToString());
                return View();
            }
        }

        // GET: Login/ViewLoginHistory
        public ActionResult ViewLoginHistory()
        {
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