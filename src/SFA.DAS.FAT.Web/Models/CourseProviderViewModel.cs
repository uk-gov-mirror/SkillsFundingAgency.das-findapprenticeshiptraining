using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProviderViewModel
    {
        public uint ProviderId { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderNumber { get; set; }
        public string ProviderWebsite { get; set; }

        public static implicit operator CourseProviderViewModel(Provider provider)
        {
            return new CourseProviderViewModel
            {
                ProviderId = provider.ProviderId,
                Name = provider.Name,
            };
        }
    }
}
