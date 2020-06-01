using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}