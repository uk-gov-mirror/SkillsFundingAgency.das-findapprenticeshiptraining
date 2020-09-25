﻿using System;
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
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;
        private readonly ICookieStorageService<LocationCookieItem> _cookieStorageService;

        public CoursesController (
            ILogger<CoursesController> logger,
            IMediator mediator,
            ICookieStorageService<LocationCookieItem> cookieStorageService)
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
            var location = _cookieStorageService.Get(Constants.LocationCookieName);
            var result = await _mediator.Send(new GetCourseQuery
            {
                CourseId = id,
                Lat = location?.Lat ?? 0,
                Lon = location?.Lon ?? 0
            });
            
            var viewModel = (CourseViewModel)result.Course;
            viewModel.LocationName = location.Name;
            viewModel.TotalProvidersCount = result.ProvidersCount?.TotalProviders;
            viewModel.ProvidersAtLocationCount = result.ProvidersCount?.ProvidersAtLocation;
            
            return View(viewModel);
        }

        [Route("{id}/providers", Name = RouteNames.CourseProviders)]
        public async Task<IActionResult> CourseProviders(int id, string location, ProviderSortBy sortOrder)
        {
            try
            {
                location = CheckLocation(location);
                
                var result = await _mediator.Send(new GetCourseProvidersQuery
                {
                    CourseId = id,
                    Location = location,
                    SortOrder = sortOrder
                });
                
                var cookieResult =new LocationCookieItem
                {
                    Name = result.Location,
                    Lat = result.LocationGeoPoint?.FirstOrDefault() ??0,
                    Lon = result.LocationGeoPoint?.LastOrDefault() ??0
                }; 
                UpdateLocationCookie(cookieResult);
                
                return View(new CourseProvidersViewModel
                {
                    Course = result.Course,
                    Providers = result.Providers.Select(c=>(ProviderViewModel)c), 
                    Total = result.Total,
                    Location = cookieResult.Name,
                    SortOrder = sortOrder,
                    HasLocations = !string.IsNullOrEmpty(result.Location)
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
                _cookieStorageService.Delete(Constants.LocationCookieName);
                return "";
            }
            if (string.IsNullOrEmpty(location))
            {
                location = _cookieStorageService.Get(Constants.LocationCookieName)?.Name;
            }

            return location;
        }
        private void UpdateLocationCookie(LocationCookieItem location)
        {
            if (!string.IsNullOrEmpty(location.Name) && location.Lat != 0 && location.Lon != 0)
            {
                _cookieStorageService.Update(Constants.LocationCookieName, location, 2);    
            }
        }
    }
}
