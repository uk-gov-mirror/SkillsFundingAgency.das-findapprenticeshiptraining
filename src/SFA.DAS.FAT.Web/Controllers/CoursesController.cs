using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Courses()
        {
            return View();
        }
    }
}