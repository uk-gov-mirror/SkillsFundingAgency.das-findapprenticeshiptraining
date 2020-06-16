using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;

namespace SFA.DAS.FAT.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Courses()
        {
            var courses = _mediator.Send(new GetCoursesRequest());
            return View();
        }
    }
}