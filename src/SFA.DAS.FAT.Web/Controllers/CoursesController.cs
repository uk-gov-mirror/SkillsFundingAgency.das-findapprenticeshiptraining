using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Web.Infrastructure;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;
        private readonly ICookieStorageService<LocationCookieItem> _locationCookieStorageService;
        private readonly ICookieStorageService<GetCourseProvidersRequest> _courseProvidersCookieStorageService;

        public CoursesController (
            ILogger<CoursesController> logger,
            IMediator mediator,
            ICookieStorageService<LocationCookieItem> locationCookieStorageService,
            ICookieStorageService<GetCourseProvidersRequest> courseProvidersCookieStorageService)
        {
            _logger = logger;
            _mediator = mediator;
            _locationCookieStorageService = locationCookieStorageService;
            _courseProvidersCookieStorageService = courseProvidersCookieStorageService;
        }

        [Route("", Name = RouteNames.Courses)]
        public async Task<IActionResult> Courses(GetCoursesRequest request)
        {
            var result = await _mediator.Send(new GetCoursesQuery
            {
                Keyword = request.Keyword,
                RouteIds = request.Sectors,
                Levels = request.Levels,
                OrderBy = request.OrderBy
            });

            var viewModel = new CoursesViewModel
            {
                Courses = result.Courses.Select(c => (CourseViewModel)c).ToList(),
                Sectors = result.Sectors.Select(sector => new SectorViewModel(sector, request.Sectors)).ToList(),
                Total = result.Total,
                TotalFiltered = result.TotalFiltered,
                Keyword = request.Keyword,
                SelectedSectors = request.Sectors,
                SelectedLevels = request.Levels,
                Levels = result.Levels.Select(level => new LevelViewModel(level, request.Levels)).ToList(),
                OrderBy = request.OrderBy
            };

            return View(viewModel);
        }

        [Route("{id}", Name = RouteNames.CourseDetails)]
        public async Task<IActionResult> CourseDetail(int id, [FromQuery(Name="location")]string locationName)
        {
            CheckLocation(locationName);
            var location = _locationCookieStorageService.Get(Constants.LocationCookieName);
            var result = await _mediator.Send(new GetCourseQuery
            {
                CourseId = id,
                Lat = location?.Lat ?? 0,
                Lon = location?.Lon ?? 0
            });
            
            var viewModel = (CourseViewModel)result.Course;
            viewModel.LocationName = location?.Name;
            viewModel.TotalProvidersCount = result.ProvidersCount?.TotalProviders;
            viewModel.ProvidersAtLocationCount = result.ProvidersCount?.ProvidersAtLocation;
            
            return View(viewModel);
        }

        [Route("{id}/providers", Name = RouteNames.CourseProviders)]
        public async Task<IActionResult> CourseProviders(GetCourseProvidersRequest request)
        {
            try
            {
                var location = CheckLocation(request.Location);
                
                var result = await _mediator.Send(new GetCourseProvidersQuery
                {
                    CourseId = request.Id,
                    Location = location,
                    DeliveryModes = request.DeliveryModes.Select(type => (Domain.Courses.DeliveryModeType)type),
                    ProviderRatings = request.ProviderRatings.Select(rating => (Domain.Courses.ProviderRating)rating)
                });
                
                var cookieResult =new LocationCookieItem
                {
                    Name = result.Location,
                    Lat = result.LocationGeoPoint?.FirstOrDefault() ??0,
                    Lon = result.LocationGeoPoint?.LastOrDefault() ??0
                }; 
                UpdateLocationCookie(cookieResult);

                _courseProvidersCookieStorageService.Create(request, nameof(GetCourseProvidersRequest));
                
                return View(new CourseProvidersViewModel(request, result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.Error500);
            }
        }

        [Route("{id}/providers/{providerId}", Name = RouteNames.CourseProviderDetails)]
        public async Task<IActionResult> CourseProviderDetail(int id, int providerId, string location)
        {
            try
            {
                location = CheckLocation(location);
                var result = await _mediator.Send(new GetCourseProviderQuery
                {
                    ProviderId = providerId ,
                    CourseId = id, 
                    Location = location
                });

                var cookieResult =new LocationCookieItem
                {
                    Name = result.Location,
                    Lat = result.LocationGeoPoint?.FirstOrDefault() ?? 0,
                    Lon = result.LocationGeoPoint?.LastOrDefault() ?? 0
                }; 
                UpdateLocationCookie(cookieResult);

                var viewModel = (CourseProviderViewModel)result;
                viewModel.Location = cookieResult.Name;
                var providersRequestCookie = _courseProvidersCookieStorageService.Get(nameof(GetCourseProvidersRequest));
                if (providersRequestCookie != default)
                {
                    providersRequestCookie.Location = location;
                    viewModel.GetCourseProvidersRequest = providersRequestCookie.ToDictionary();
                }

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.Error500);
            }
        }

        private string CheckLocation(string location)
        {
            if (location == "-1")
            {
                _locationCookieStorageService.Delete(Constants.LocationCookieName);
                return "";
            }
            if (string.IsNullOrEmpty(location))
            {
                location = _locationCookieStorageService.Get(Constants.LocationCookieName)?.Name;
            }

            return location;
        }

        private void UpdateLocationCookie(LocationCookieItem location)
        {
            if (!string.IsNullOrEmpty(location.Name) && location.Lat != 0 && location.Lon != 0)
            {
                _locationCookieStorageService.Update(Constants.LocationCookieName, location, 2);    
            }
        }
    }
}
