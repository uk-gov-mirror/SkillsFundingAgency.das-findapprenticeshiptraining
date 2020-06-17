using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesRequestHandler : IRequestHandler<GetCoursesRequest, GetCoursesResult>
    {
        private readonly ICourseService _courseService;

        public GetCoursesRequestHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<GetCoursesResult> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
        {
            var response = await _courseService.GetCourses();

            return new GetCoursesResult{Courses = response.ToList()};
        }
    }
}