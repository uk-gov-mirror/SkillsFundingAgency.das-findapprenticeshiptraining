using System;
using System.Collections.Generic;
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
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Extensions;
using SFA.DAS.FAT.Domain.Interfaces;
using DeliveryModeType = SFA.DAS.FAT.Web.Models.DeliveryModeType;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;
        private readonly ICookieStorageService<string> _cookieStorageService;

        public CoursesController (
            ILogger<CoursesController> logger,
            IMediator mediator,
            ICookieStorageService<string> cookieStorageService)
        {
            _logger = logger;
            _mediator = mediator;
            _cookieStorageService = cookieStorageService;
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
        public async Task<IActionResult> CourseDetail(int id)
        {
            var result = await _mediator.Send(new GetCourseQuery {CourseId = id});
            
            var viewModel = (CourseViewModel)result.Course;
            viewModel.ProvidersCount = result.ProvidersCount;
            
            return View(viewModel);
        }

        [Route("{id}/providers", Name = RouteNames.CourseProviders)]
        public async Task<IActionResult> CourseProviders(int id, string location, IReadOnlyList<DeliveryModeType> deliveryModes, ProviderSortBy sortOrder)
        {
            try
            {
                location = UpdateLocationCookie(location);
                
                var result = await _mediator.Send(new GetCourseProvidersQuery
                {
                    CourseId = id,
                    Location = location,
                    DeliveryModes = deliveryModes.Select(type => (Domain.Courses.DeliveryModeType)type),
                    SortOrder = sortOrder
                });

                return View(new CourseProvidersViewModel
                {
                    Course = result.Course,
                    Providers = result.Providers.Select(c=>(ProviderViewModel)c), 
                    Total = result.Total,
                    Location = location,
                    SortOrder = sortOrder,
                    DeliveryModes = BuildDeliveryModeOptionViewModel(deliveryModes)
                });
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
                location = UpdateLocationCookie(location);
                
                var result = await _mediator.Send(new GetCourseProviderQuery
                {
                    ProviderId = providerId ,
                    CourseId = id, 
                    Location = location
                });

                var viewModel = (CourseProviderViewModel)result;

                viewModel.Location = location;
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.Error500);
            }
        }

        private string UpdateLocationCookie(string location)
        {
            if (location == "-1")
            {
                _cookieStorageService.Delete(Constants.LocationCookieName);
                return string.Empty;
            }
            
            if (string.IsNullOrEmpty(location))
            {
                location = _cookieStorageService.Get(Constants.LocationCookieName);
            }
            
            _cookieStorageService.Update(Constants.LocationCookieName, location, 2);
            
            return location;
        }

        private static IEnumerable<DeliveryModeOptionViewModel> BuildDeliveryModeOptionViewModel(IReadOnlyList<DeliveryModeType> selectedDeliveryModeTypes)
        {
            var deliveryModeOptionViewModels = new List<DeliveryModeOptionViewModel>();

            foreach (DeliveryModeType deliveryModeType in Enum.GetValues(typeof(DeliveryModeType)))
            {
                deliveryModeOptionViewModels.Add(new DeliveryModeOptionViewModel
                {
                    DeliveryModeType = deliveryModeType,
                    Description = deliveryModeType.GetDescription(),
                    Selected = selectedDeliveryModeTypes.Any(type => type == deliveryModeType)
                });
            }

            return deliveryModeOptionViewModels;
        }
    }
}
