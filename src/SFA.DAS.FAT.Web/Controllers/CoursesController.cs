using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Web.Infrastructure;
﻿using System.Threading.Tasks;

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
        public IActionResult Courses()
        {
            var courses = _mediator.Send(new GetCoursesRequest());

            var viewModel = new CourseViewModel {};
            return View(viewModel);
        }

        [Route("{id}", Name = RouteNames.TrainingDetail)]
        public async Task<IActionResult> CourseDetail(int id)
        {
            var result = await _mediator.Send(new GetCourseQuery {CourseId = id});
            
            var viewModel = (CourseDetailViewModel)result.Course;
            
            return View(viewModel);
        }
    }
}