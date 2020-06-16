using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseRequest : IRequest<GetCourseResult>
    {
        public int CourseId { get ; set ; }
    }
}