using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_1.Controllers
{
    public class profileController : Controller
    {
        // GET: profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult cards()
        {
            return View();
        }
    }
}