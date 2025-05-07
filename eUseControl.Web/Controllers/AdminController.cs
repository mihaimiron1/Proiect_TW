using System.Web.Mvc;
using eUseControl.Web.Models;
using System.Web;
using System.Linq;

namespace eUseControl.Web.Controllers
{
    public class AdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Check if user is logged in and is admin
            if (Session["UserId"] == null || Session["UserRole"]?.ToString() != "Admin")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" }
                    });
            }
        }

        public ActionResult Dashboard()
        {
            using (var db = new eUseControl.Web.Data.ApplicationDbContext())
            {
                var transfers = db.TransferCards.OrderByDescending(t => t.TransferDate).ToList();
                ViewBag.A2ATransfers = transfers;
            }
            return View();
        }
    }
} 