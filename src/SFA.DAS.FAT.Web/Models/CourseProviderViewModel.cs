using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProviderViewModel
    {
        public uint ProviderId { get; set; }
        public string Name { get; set; }
        public CourseViewModel Course { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public ProviderCoursesViewModel AdditionalCourses { get; set; }

        public static implicit operator CourseProviderViewModel(GetCourseProviderResult provider)
        {
            return new CourseProviderViewModel
            {
                ProviderId = provider.Provider.ProviderId,
                Name = provider.Provider.Name,
                Phone = provider.Provider.Phone,
                Email = provider.Provider.Email,
                Website = provider.Provider.Website,
                Course = provider.Course,
                AdditionalCourses = provider.AdditionalCourses,
            };
        }
    }
}
