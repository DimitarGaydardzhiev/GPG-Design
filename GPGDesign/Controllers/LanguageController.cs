using System.Globalization;
using System.Threading;
using System.Web;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GPGDesign.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Change(string LanguageAbbreviation)
        {
            if (LanguageAbbreviation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbreviation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbreviation);                
            }
            CookieOptions option = new CookieOptions();
            Response.Cookies.Append("Language", LanguageAbbreviation, option);

            return View("Index");
        }
    }
}
