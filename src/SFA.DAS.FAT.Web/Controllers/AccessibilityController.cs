using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Controllers
{
    public class AccessibilityController : Controller
    {
        public IActionResult AccessibilityStatement()
        {
            return View();
        }
    }
}
