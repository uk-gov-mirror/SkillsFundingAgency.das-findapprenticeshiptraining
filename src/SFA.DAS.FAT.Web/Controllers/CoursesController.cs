using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Web.Infrastructure;
using SFA.DAS.FAT.Web.Models;

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
            return View();
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