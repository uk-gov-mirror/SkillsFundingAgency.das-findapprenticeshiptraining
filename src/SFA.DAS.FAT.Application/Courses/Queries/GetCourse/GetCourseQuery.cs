using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseQuery : IRequest<GetCourseResult>
    {
        public int CourseId { get ; set ; }
        public double Lat { get ; set ; }
        public double Lon { get ; set ; }
    }
}