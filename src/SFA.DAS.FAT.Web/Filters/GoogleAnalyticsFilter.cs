using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            if (context.RouteData.Values.TryGetValue("location", out var location))
            {
                controller.ViewBag.GaData =  new GaData {Location = location.ToString()};
            }

            base.OnActionExecuting(context);
        }
    }
}
