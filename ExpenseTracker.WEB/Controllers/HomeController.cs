namespace ExpenseTracker.WEB.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _LoginPrompt()
        {
            return PartialView();
        }
    }
}