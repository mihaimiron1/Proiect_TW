using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Data;

namespace Lab_1.Controllers
{
    public class profileController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        // GET: profile
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Index", "Login");
            var email = Session["UserEmail"].ToString();
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Index", "Login");
            return View(user);
        }
       
        [HttpPost]
        public JsonResult UpdateField(string field, string value)
        {
            if (Session["UserEmail"] == null)
                return Json(new { success = false });

            var email = Session["UserEmail"].ToString();
            using (var db = new eUseControl.Web.Data.ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                    return Json(new { success = false });

                switch (field)
                {
                    case "UserName":
                        user.Name = value;
                        Session["UserName"] = value;
                        break;
                    case "UserPhone":
                        user.PhoneNumber = value;
                        Session["UserPhone"] = value;
                        break;
                    case "UserEmail":
                        user.Email = value;
                        Session["UserEmail"] = value;
                        break;
                    case "UserCity":
                        user.City = value;
                        Session["UserCity"] = value;
                        break;
                    case "UserCountry":
                        user.Country = value;
                        Session["UserCountry"] = value;
                        break;
                    default:
                        return Json(new { success = false });
                }

                db.SaveChanges();
                return Json(new { success = true });
            }
        }
    }
}