using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class ProviderCoursesViewModel
    {
        public int Total { get; set; }
        public IEnumerable<ProviderCourseViewModel> Courses { get; set; }
        
        public static implicit operator ProviderCoursesViewModel(AdditionalCourses additionalCourses)
        {
            return new ProviderCoursesViewModel
            {
                Total = additionalCourses.Total,
                Courses = additionalCourses.Courses.Select(course => (ProviderCourseViewModel)course),
            };
        }
    }
}
