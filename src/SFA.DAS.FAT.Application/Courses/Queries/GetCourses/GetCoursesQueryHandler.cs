using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, GetCoursesResult>
    {
        private readonly ICourseService _courseService;

        public GetCoursesQueryHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<GetCoursesResult> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
        {
            var response = await _courseService.GetCourses(query.Keyword, query.RouteIds, query.Levels);

            return new GetCoursesResult
            {
                Courses = response.Courses.ToList(),
                Sectors = response.Sectors.ToList(),
                Total = response.Total,
                TotalFiltered = response.TotalFiltered,
                Levels = response.Levels.ToList()
            };
        }
    }
}