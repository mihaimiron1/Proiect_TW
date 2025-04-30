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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session["UserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" },
                        { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                    });
            }
        }

        // GET: transfer
        public ActionResult a2a()
        {
            return View();
        }

        public ActionResult p2p()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult afm()
        {
            return View();
        }

        public ActionResult t2c()
        {
            return View();
        }

        public ActionResult ViewTransferA2A()
        {
            var transfers = _transferCardService.GetAllTransfers();
            return View(transfers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateA2ATransfer(TransferCard transfer)
        {
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