using System.Web.Mvc;

namespace TShirtEmpAdmin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.TabTitle = "An Error Has Occured";
            return View();
        }
        public ActionResult NotFound()
        {
            ViewBag.TabTitle = "Page Not Found";
            Response.StatusCode = 404;
            return View();
        }
    }
}