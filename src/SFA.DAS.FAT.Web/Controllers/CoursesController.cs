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
            var location = CheckLocation(locationName);
            var result = await _mediator.Send(new GetCourseQuery
            {
                CourseId = id,
                Lat = location?.Lat ?? 0,
                Lon = location?.Lon ?? 0
            });

            if (result.Course == null)
            {
                return RedirectToRoute(RouteNames.Error404);
            }
            
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
                    Location = location?.Name ?? "",
                    Lat = location?.Lat ?? 0,
                    Lon = location?.Lon ?? 0,
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
                
                if (result.Course == null)
                {
                    return RedirectToRoute(RouteNames.Error404);
                }
                
                _courseProvidersCookieStorageService.Create(request, nameof(GetCourseProvidersRequest));

                var courseProvidersViewModel = new CourseProvidersViewModel(request, result);

                if (courseProvidersViewModel.Course.AfterLastStartDate)
                {
                    return RedirectToRoute(RouteNames.CourseDetails,new {request.Id});
                }
                
                return View(courseProvidersViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.Error500);
            }
        }

        [Route("{id}/providers/{providerId}", Name = RouteNames.CourseProviderDetails)]
        public async Task<IActionResult> CourseProviderDetail(int id, int providerId, string location, string providerPlacement, string providerTotal)
        {
            try
            {
                var locationItem = CheckLocation(location);
                var result = await _mediator.Send(new GetCourseProviderQuery
                {
                    ProviderId = providerId ,
                    CourseId = id, 
                    Location = locationItem?.Name ?? "",
                    Lat = locationItem?.Lat ?? 0,
                    Lon = locationItem?.Lon ?? 0
                });

                var cookieResult =new LocationCookieItem
                {
                    Name = result.Location,
                    Lat = result.LocationGeoPoint?.FirstOrDefault() ?? 0,
                    Lon = result.LocationGeoPoint?.LastOrDefault() ?? 0
                }; 
                UpdateLocationCookie(cookieResult);

                if (result.Course == null)
                {
                    return RedirectToRoute(RouteNames.Error404);
                }
                
                var viewModel = (CourseProviderViewModel)result;
                viewModel.Location = cookieResult.Name;
                var providersRequestCookie = _courseProvidersCookieStorageService.Get(nameof(GetCourseProvidersRequest));
                if (providersRequestCookie != default)
                {
                    providersRequestCookie.Location = result?.Location;
                    viewModel.GetCourseProvidersRequest = providersRequestCookie.ToDictionary();
                }

                if (viewModel.Course.AfterLastStartDate)
                {
                    return RedirectToRoute(RouteNames.CourseDetails,new {Id = id});
                }

                if (viewModel.Provider == null)
                {
                    return RedirectToRoute(RouteNames.Error404);
                }
                viewModel.ProviderPlacement = int.Parse(providerPlacement);
                viewModel.ProviderTotal = int.Parse(providerTotal);
                
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.Error500);
            }
        }

        private LocationCookieItem CheckLocation(string location)
        {
            if (location == "-1")
            {
                _locationCookieStorageService.Delete(Constants.LocationCookieName);
                return null;
            }
            if (string.IsNullOrEmpty(location))
            {
                return _locationCookieStorageService.Get(Constants.LocationCookieName);
            }

            return new LocationCookieItem
            {
                Name = location
            };
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
