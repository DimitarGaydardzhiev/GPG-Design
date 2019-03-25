using Microsoft.AspNetCore.Mvc;

namespace GPGDesign.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
