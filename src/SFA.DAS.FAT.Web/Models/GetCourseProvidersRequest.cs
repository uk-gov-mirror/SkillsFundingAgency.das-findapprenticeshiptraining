using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class GetCourseProvidersRequest
    {
        public int Id { get; set; }
        [FromQuery]
        public string Location { get; set; }
        [FromQuery]
        public IReadOnlyList<DeliveryModeType> DeliveryModes { get; set; } = new List<DeliveryModeType>();
        [FromQuery]
        public IReadOnlyList<ProviderRating> ProviderRatings { get; set; } = new List<ProviderRating>();
        
    }
}
