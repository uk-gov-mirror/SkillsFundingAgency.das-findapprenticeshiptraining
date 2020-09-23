using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders
{
    public class GetCourseProvidersResult
    {
        public Course Course { get; set; }
        public IEnumerable<Provider> Providers { get; set; }
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public string Location { get; set; }
    }
}
