using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Web.Infrastructure;
﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("courses")]
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController (IMediator mediator)
        {
            _mediator = mediator;
        }
        [Route("", Name = RouteNames.Training)]
        public async Task<IActionResult> Courses(GetCoursesRequest request)
        {
            var result = await _mediator.Send(new GetCoursesQuery
            {
                Keyword = request.Keyword,
                RouteIds = request.Sectors
            });

            var viewModel = new CoursesViewModel
            {
                Courses = result.Courses.Select(c=>(CourseViewModel)c).ToList(),
                Sectors = result.Sectors.Select(sector => new SectorViewModel(sector, request.Sectors)).ToList(),
                Total = result.Total,
                TotalFiltered = result.TotalFiltered,
                Keyword = request.Keyword
            };
            
            return View(viewModel);
        }

        [Route("{id}", Name = RouteNames.TrainingDetail)]
        public async Task<IActionResult> CourseDetail(int id)
        {
            var result = await _mediator.Send(new GetCourseQuery {CourseId = id});
            
            var viewModel = (CourseViewModel)result.Course;
            
            return View(viewModel);
        }
    }
}