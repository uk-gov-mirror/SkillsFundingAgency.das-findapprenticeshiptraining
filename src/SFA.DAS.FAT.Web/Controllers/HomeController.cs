using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Web.Infrastructure;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("service")]
    public class HomeController : Controller
    {
        [Route("",Name = RouteNames.ServiceStart)]
        public IActionResult Index()
        {
            return View();
        }
    }
}