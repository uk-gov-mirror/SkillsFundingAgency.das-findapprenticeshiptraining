using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        private readonly ICookieStorageService<LocationCookieItem> _locationCookieStorageService;
        private readonly ICookieStorageService<GetCourseProvidersRequest> _courseProviderRequestCookieStorageService;

        public GoogleAnalyticsFilter(ICookieStorageService<LocationCookieItem> locationCookieStorageService, 
            ICookieStorageService<GetCourseProvidersRequest> courseProviderRequestCookieStorageService)
        {
            _locationCookieStorageService = locationCookieStorageService;
            _courseProviderRequestCookieStorageService = courseProviderRequestCookieStorageService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            var gaData = new GaData();
            var locationFromCookie = _locationCookieStorageService.Get(Constants.LocationCookieName);
            
            if (context.HttpContext.Request.Query.TryGetValue("location", out var location))
            {
                gaData.Location = location.ToString();
            }
            else if (locationFromCookie != null)
            {
                if (!string.IsNullOrEmpty(locationFromCookie.Name) && locationFromCookie.Lat != 0 &&
                    locationFromCookie.Lon != 0)
                {
                    gaData.Location = locationFromCookie.Name;
                }
            }
            
            
            if (context.RouteData.Values.TryGetValue("providerId", out var providerId))
            {
                if (uint.TryParse(providerId.ToString(), out var ukprn))
                {
                    var requestData = _courseProviderRequestCookieStorageService.Get(nameof(GetCourseProvidersRequest));
                    if (requestData != null)
                    {
                        var providerList = requestData.Providers.Split('|').ToList();
                        gaData.ProviderPlacement = providerList.IndexOf(ukprn.ToString());
                        gaData.ProviderTotal = providerList.Count;
                    }    
                }
            }

            controller.ViewBag.GaData = gaData;

            base.OnActionExecuting(context);
        }
    }
}
