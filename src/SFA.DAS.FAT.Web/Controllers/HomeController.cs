using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Web.Infrastructure;

namespace SFA.DAS.FAT.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
        [Route("start", Name = RouteNames.ServiceStart, Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("cookies", Name = RouteNames.Cookies)]
        public IActionResult Cookies()
        {
            return View();
        }

        [Route("cookie-details", Name = RouteNames.CookieDetails)]
        public IActionResult CookieDetails()
        {
            return View();
        }
    }
}
