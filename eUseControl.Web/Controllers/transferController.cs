using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eUseControl.Web.Models;
using eUseControl.Web.Services;

namespace Lab_1.Controllers
{
    public class transferController : Controller
    {
        private readonly TransferCardService _transferCardService;

        public transferController()
        {
            _transferCardService = new TransferCardService();
        }

        private bool IsUserAuthenticated()
        {
            return Session["UserId"] != null;
        }

        private ActionResult RedirectToLogin()
        {
            return new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Index" },
                    { "returnUrl", Request.RawUrl }
                });
        }

        // GET: transfer
        public ActionResult Index()
        {
            // Allow viewing the index page without authentication
            ViewBag.IsAuthenticated = IsUserAuthenticated();
            return View();
        }

        public ActionResult a2a()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult p2p()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult afm()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult t2c()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult ViewTransferA2A()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            var transfers = _transferCardService.GetAllTransfers();
            return View(transfers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateA2ATransfer(TransferCard transfer)
        {
            if (!IsUserAuthenticated())
                return Json(new { success = false, error = "Authentication required" });

            try
            {
                if (ModelState.IsValid)
                {
                    transfer.BankSender = "Maib";
                    transfer.BankBeneficiary = "Maib";
                    transfer.TransferDate = DateTime.Now;
                    
                    _transferCardService.CreateTransfer(transfer);
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}