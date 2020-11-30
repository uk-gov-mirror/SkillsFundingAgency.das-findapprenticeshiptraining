using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        private readonly ICookieStorageService<LocationCookieItem> _locationCookieStorageService;
        public GoogleAnalyticsFilter(ICookieStorageService<LocationCookieItem> locationCookieStorageService)
        {
            _locationCookieStorageService = locationCookieStorageService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            var locationFromCookie = _locationCookieStorageService.Get(Constants.LocationCookieName);
            
            if (context.HttpContext.Request.Query.TryGetValue("location", out var location))
            {
                controller.ViewBag.GaData =  new GaData {Location = location.ToString()};
            }
            else if (locationFromCookie != null)
            {
                if (!string.IsNullOrEmpty(locationFromCookie.Name) && locationFromCookie.Lat != 0 &&
                    locationFromCookie.Lon != 0)
                {
                    controller.ViewBag.GaData = new GaData {Location = locationFromCookie.Name};
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
