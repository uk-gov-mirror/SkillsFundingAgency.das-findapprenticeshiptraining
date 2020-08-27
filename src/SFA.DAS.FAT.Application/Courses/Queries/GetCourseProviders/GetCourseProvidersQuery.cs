using MediatR;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders
{
    public class GetCourseProvidersQuery: IRequest<GetCourseProvidersResult>
    {
        public int CourseId { get; set; }
        public string Location { get ; set ; }
        public ProviderSortBy SortOrder { get ; set ; }
    }
}