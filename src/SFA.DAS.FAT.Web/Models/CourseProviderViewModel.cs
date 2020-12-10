using System.Collections.Generic;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProviderViewModel
    {
        public CourseViewModel Course { get; set; }
        public ProviderViewModel Provider { get; set; }
        public ProviderCoursesViewModel AdditionalCourses { get; set; }
        public string Location { get ; set ; }
        public int TotalProviders { get ; set ; }
        public Dictionary<string, string> GetCourseProvidersRequest { get; set; } = new Dictionary<string, string>();
        public int? ProviderPlacement { get; set; } = 0;
        public int? ProviderTotal { get; set; } = 0;

        public static implicit operator CourseProviderViewModel(GetCourseProviderResult provider)
        {
            return new CourseProviderViewModel
            {
                Provider = provider.Provider,
                Course = provider.Course,
                AdditionalCourses = provider.AdditionalCourses,
                TotalProviders = provider.ProvidersAtLocation 
            };
        }
    }
}
