using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<GetCoursesResult>
    {
        public string Keyword { get; set; }
        public List<Guid> RouteIds { get; set; }
        public List<int> Codes { get; set; }
    }
}