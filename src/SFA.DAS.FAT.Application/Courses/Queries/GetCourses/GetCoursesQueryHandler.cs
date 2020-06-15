using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, GetCoursesResponse>
    {
        public Task<GetCoursesResponse> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}