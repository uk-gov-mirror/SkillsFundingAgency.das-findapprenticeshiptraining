using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Web.Infrastructure;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("controller")]
    public class AccessibilityController : Controller
    {
        [Route("", Name = RouteNames.AccessibilityStatement)]
        public IActionResult AccessibilityStatement()
        {
            return View();
        }
    }
}
