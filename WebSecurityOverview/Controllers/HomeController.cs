using System.Web.Mvc;

namespace WebSecurityOverview.Controllers
{
    using WebSecurityOverview.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new MyModel());
        }
    }
}