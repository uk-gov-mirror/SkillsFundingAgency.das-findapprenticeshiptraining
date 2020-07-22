using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourses
{
    public class GetCoursesResult 
    {
        public List<Course> Courses { get; set; }
        public List<Sector> Sectors { get ; set ; }
        public List<Level> Levels { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
        public OrderBy OrderBy { get; set; }
    }
}