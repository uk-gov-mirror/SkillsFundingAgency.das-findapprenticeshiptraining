using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<GetCoursesResult>
    {
        public string Keyword { get; set; }
    }
}