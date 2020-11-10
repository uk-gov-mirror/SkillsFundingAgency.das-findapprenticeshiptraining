using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetCourseProviderQuery : IRequest<GetCourseProviderResult>
    {
        public int ProviderId { get; set; }
        public int CourseId { get ; set ; }
        public string Location { get ; set ; }
        public double Lon { get ; set ; }
        public double Lat { get ; set ; }
    }
}
