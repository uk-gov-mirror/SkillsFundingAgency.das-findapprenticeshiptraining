using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetCourseProviderQuery : IRequest<GetCourseProviderResult>
    {
        public int ProviderId { get; set; }
        public int CourseId { get ; set ; }
    }
}
