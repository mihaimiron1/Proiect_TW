using eUseControl.BusinessLogic.Interface;
using eUserControl.Domain.Entities.User;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISession _session;
        public LoginController()
        {
            var bl = new BussinesLogin();
            _session = bl.GetSessionBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                ULoginData data = new ULoginData
                {
                    Credential = login.Credential,
                    Password = login.Password,
                    LoginIn = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now
                };
                var userLogin -_session.UserLogin(data);
                if (userLogin.Status)
                {
                    //Add cookie
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", userLoginStatusMsg);
                    return View();
                }
            }
            return View();
        }




    }
}