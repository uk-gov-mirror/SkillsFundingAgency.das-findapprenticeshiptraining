using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders
{
    public class GetCourseProvidersQuery: IRequest<GetCourseProvidersResult>
    {
        public int CourseId { get; set; }
    }
}