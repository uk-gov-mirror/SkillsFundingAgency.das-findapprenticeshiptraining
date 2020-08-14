using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetProvider
{
    public class GetCourseProviderResult
    {
        public Provider Provider { get; set; }
        public Course Course { get ; set ; }
        public AdditionalCourses AdditionalCourses { get; set; }
    }
}
