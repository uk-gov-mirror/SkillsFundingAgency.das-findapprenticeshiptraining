using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Web.Infrastructure;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("[controller]")]
    public class ShortlistController : Controller
    {
        [HttpGet]
        [Route("{id}", Name = RouteNames.ShortList)]
        public IActionResult Index(Guid id)
        {
            return View();
        }
    }
}